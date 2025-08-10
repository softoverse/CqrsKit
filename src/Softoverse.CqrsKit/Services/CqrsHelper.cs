using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Abstractions.Handlers.Markers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models.Entity;

namespace Softoverse.CqrsKit.Services;

public static class CqrsHelper
{
    private static readonly List<Assembly> ApplicationAssemblies = [typeof(Type).Assembly, typeof(IBaseHandlerMarker).Assembly];
    internal static readonly List<Assembly> ConsumerAssemblies = [];
    private static readonly ConcurrentDictionary<string, Type?> CachedTypes = new ConcurrentDictionary<string, Type?>();

    public static void AddAssembly(Assembly assembly)
    {
        ApplicationAssemblies.Add(assembly);
        ConsumerAssemblies.Add(assembly);
    }

    public static string GetFullTypeName(string nameSpace, string className)
    {
        return string.IsNullOrEmpty(nameSpace)
            ? className
            : string.IsNullOrEmpty(className)
                ? string.Empty
                : $"{nameSpace}.{className}";
    }

    public static Type? GetType(string nameSpace, string className, bool throwIfNotFound = true)
    {
        var fullName = GetFullTypeName(nameSpace, className);
        return GetType(fullName, throwIfNotFound);
    }

    public static Type? GetType(string fullName, bool throwIfNotFound = true)
    {
        CachedTypes.TryGetValue(fullName, out Type? type);

        if (type == null)
        {
            type = ApplicationAssemblies.Select(item => item.GetType(fullName))
                                        .FirstOrDefault(t => t is not null);

            if (type is not null)
            {
                CacheType(fullName, type);
            }
        }

        return throwIfNotFound && type is null ? throw new NullReferenceException($"Could not find type for '{fullName}'.") : type;
    }

    private static void CacheType(string fullName, Type? type)
    {
        CachedTypes[fullName] = type;
    }

    public static IEnumerable<BaseCommandQuery<TKey>> GetAllCommandQueryTypes<TKey>() where TKey : IEquatable<TKey>
    {
        List<BaseCommandQuery<TKey>> commands = new List<BaseCommandQuery<TKey>>();
        List<BaseCommandQuery<TKey>> queries = new List<BaseCommandQuery<TKey>>();

        foreach (var assembly in ConsumerAssemblies)
        {
            commands.AddRange(GetAllCommandTypes<TKey>(assembly));
            queries.AddRange(GetAllQueryTypes<TKey>(assembly));
        }
        return commands.Concat(queries);
    }

    public static IEnumerable<BaseCommandQuery<TKey>> GetAllCommandTypes<TKey>(Assembly assembly) where TKey : IEquatable<TKey>
    {
        IEnumerable<Type> allCommandHandlerTypes = assembly.GetTypes()
                                                           .Where(type =>
                                                                      type.GetTypeInfo().IsClass &&
                                                                      typeof(ICommandHandlerMarker).IsAssignableFrom(type)
                                                                    &&
                                                                      type != typeof(ICommandHandlerMarker)
                                                                    &&
                                                                      type.Name != typeof(ICommandHandler<,>).Name);

        IEnumerable<BaseCommandQuery<TKey>> commands = allCommandHandlerTypes.Select(x =>
        {
            (Type? commandQueryType, Type? responseType) = GetCommandOrQueryAndResponseType(GetHandlerGenerics<ICommandHandlerMarker>(x));

            return ToBaseCommand<TKey>(commandQueryType, responseType);
        });

        return commands;
    }

    public static IEnumerable<BaseCommandQuery<TKey>> GetAllQueryTypes<TKey>(Assembly assembly) where TKey : IEquatable<TKey>
    {
        IEnumerable<Type> allCommandHandlerTypes = assembly.GetTypes()
                                                           .Where(type =>
                                                                      type.GetTypeInfo().IsClass &&
                                                                      typeof(IQueryHandlerMarker).IsAssignableFrom(type)
                                                                    &&
                                                                      type != typeof(IQueryHandlerMarker)
                                                                    &&
                                                                      type.Name != typeof(ICommandHandler<,>).Name);

        IEnumerable<BaseCommandQuery<TKey>> queries = allCommandHandlerTypes.Select(x =>
        {
            (Type? commandQueryType, Type? responseType) = GetCommandOrQueryAndResponseType(GetHandlerGenerics<IQueryHandlerMarker>(x));

            return ToBaseQuery<TKey>(commandQueryType, responseType);
        });

        return queries;
    }

    private static Type[]? GetHandlerGenerics<THandlerMarker>(Type baseType) where THandlerMarker : IBaseHandlerMarker
    {
        return baseType.GetInterfaces()
                       .FirstOrDefault(type => typeof(THandlerMarker).IsAssignableFrom(type))?
                       .GetGenericArguments();
    }

    private static (Type? commandQueryType, Type? responseType) GetCommandOrQueryAndResponseType(Type[]? handlerGenerics)
    {
        Type? commandQueryType = null;
        Type? responseType = null;

        if (handlerGenerics?.Length == 1)
        {
            commandQueryType = handlerGenerics[0];
        }
        else if (handlerGenerics?.Length == 2)
        {
            commandQueryType = handlerGenerics[0];
            responseType = handlerGenerics[1];
        }

        return (commandQueryType, responseType);
    }

    private static BaseCommandQuery<TKey> ToBaseCommand<TKey>(Type? commandQueryType, Type? responseType) where TKey : IEquatable<TKey>
    {
        return ToBaseCommandQuery<TKey>(commandQueryType, responseType, isCommand: true)!;
    }

    private static BaseCommandQuery<TKey> ToBaseQuery<TKey>(Type? commandQueryType, Type? responseType) where TKey : IEquatable<TKey>
    {
        return ToBaseCommandQuery<TKey>(commandQueryType, responseType, isCommand: false)!;
    }

    private static BaseCommandQuery<TKey>? ToBaseCommandQuery<TKey>(Type? commandQueryType, Type? responseType, bool isCommand) where TKey : IEquatable<TKey>
    {
        try
        {
            DescriptionAttribute? descriptionAttribute = commandQueryType?.GetCustomAttribute<DescriptionAttribute>();
            GroupAttribute? groupAttribute = commandQueryType?.GetCustomAttribute<GroupAttribute>();

            BaseCommandQuery<TKey> baseCommandQuery = new BaseCommandQuery<TKey>()
            {
                Name = commandQueryType?.Name!,
                Namespace = commandQueryType?.Namespace!,
                FullName = commandQueryType?.FullName!,
                ResponseName = responseType?.Name,
                ResponseNamespace = responseType?.Namespace,
                ResponseFullName = responseType?.FullName,
                Description = descriptionAttribute?.Description,
                Group = groupAttribute?.Name,
                IsCommand = isCommand,
                IsApprovalFlowRequired = false
            };

            return baseCommandQuery;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static TChild ToChildOfBaseCommandQuery<TChild, TKey>(BaseCommandQuery<TKey> baseCommandQuery) where TChild : BaseCommandQuery<TKey>, new() where TKey : IEquatable<TKey>
    {
        try
        {
            TChild child = new TChild
            {
                Id = baseCommandQuery.Id,
                Name = baseCommandQuery.Name,
                Namespace = baseCommandQuery.Namespace,
                FullName = baseCommandQuery.FullName,
                ResponseName = baseCommandQuery.ResponseName,
                ResponseNamespace = baseCommandQuery.ResponseNamespace,
                ResponseFullName = baseCommandQuery.ResponseFullName,
                Description = baseCommandQuery.Description,
                IsCommand = baseCommandQuery.IsCommand,
                IsApprovalFlowRequired = baseCommandQuery.IsApprovalFlowRequired
            };

            return child;
        }
        catch (Exception ex)
        {
            return null!;
        }
    }

    public static JsonSerializerOptions GetDefaultSerializerOption()
    {
        return new JsonSerializerOptions();
    }
}