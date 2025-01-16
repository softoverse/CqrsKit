using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Abstraction.Handlers.Markers;
using Softoverse.CqrsKit.Model.Entity;

namespace Softoverse.CqrsKit.Services;

public static class CqrsHelper
{
    private static readonly List<Assembly> ApplicationAssemblies = [typeof(Type).Assembly, typeof(IBaseHandlerMarker).Assembly];
    private static readonly ConcurrentDictionary<string, Type?> CachedTypes = new ConcurrentDictionary<string, Type?>();

    public static void AddAssembly(Assembly assembly)
    {
        ApplicationAssemblies.Add(assembly);
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

        if (throwIfNotFound && type is null)
        {
            throw new NullReferenceException($"Could not find type for '{fullName}'.");
        }

        return type;
    }

    private static void CacheType(string fullName, Type? type)
    {
        CachedTypes[fullName] = type;
    }

    public static IEnumerable<BaseCommandQuery> GetAllCommandQueryTypes(Assembly assembly)
    {
        IEnumerable<BaseCommandQuery> commands = GetAllCommandTypes(assembly);
        IEnumerable<BaseCommandQuery> queries = GetAllQueryTypes(assembly);

        return commands.Concat(queries);
    }

    public static IEnumerable<BaseCommandQuery> GetAllCommandTypes(Assembly assembly)
    {
        IEnumerable<Type> allCommandHandlerTypes = assembly.GetTypes()
                                                           .Where(type =>
                                                                      type.GetTypeInfo().IsClass &&
                                                                      typeof(ICommandHandlerMarker).IsAssignableFrom(type)
                                                                    &&
                                                                      type != typeof(ICommandHandlerMarker)
                                                                    &&
                                                                      type.Name != typeof(ICommandHandler<,>).Name);

        IEnumerable<BaseCommandQuery> commands = allCommandHandlerTypes.Select(x =>
        {
            (Type? commandQueryType, Type? responseType) = GetCommandOrQueryAndResponseType(GetHandlerGenerics<ICommandHandlerMarker>(x));

            return ToBaseCommand(commandQueryType, responseType);
        });

        return commands;
    }

    public static IEnumerable<BaseCommandQuery> GetAllQueryTypes(Assembly assembly)
    {
        IEnumerable<Type> allCommandHandlerTypes = assembly.GetTypes()
                                                           .Where(type =>
                                                                      type.GetTypeInfo().IsClass &&
                                                                      typeof(IQueryHandlerMarker).IsAssignableFrom(type)
                                                                    &&
                                                                      type != typeof(IQueryHandlerMarker)
                                                                    &&
                                                                      type.Name != typeof(ICommandHandler<,>).Name);

        IEnumerable<BaseCommandQuery> queries = allCommandHandlerTypes.Select(x =>
        {
            (Type? commandQueryType, Type? responseType) = GetCommandOrQueryAndResponseType(GetHandlerGenerics<IQueryHandlerMarker>(x));

            return ToBaseQuery(commandQueryType, responseType);
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

    private static BaseCommandQuery ToBaseCommand(Type? commandQueryType, Type? responseType)
    {
        return ToBaseCommandQuery(commandQueryType, responseType, isCommand: true)!;
    }

    private static BaseCommandQuery ToBaseQuery(Type? commandQueryType, Type? responseType)
    {
        return ToBaseCommandQuery(commandQueryType, responseType, isCommand: false)!;
    }

    private static BaseCommandQuery? ToBaseCommandQuery(Type? commandQueryType, Type? responseType, bool isCommand)
    {
        try
        {
            DescriptionAttribute? descriptionAttribute = commandQueryType?.GetCustomAttribute<DescriptionAttribute>();

            BaseCommandQuery baseCommandQuery = new BaseCommandQuery()
            {
                Name = commandQueryType?.Name!,
                Namespace = commandQueryType?.Namespace!,
                FullName = commandQueryType?.FullName!,
                ResponseName = responseType?.Name,
                ResponseNamespace = responseType?.Namespace,
                ResponseFullName = responseType?.FullName,
                Description = descriptionAttribute?.Description,
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

    public static TChild ToChildOfBaseCommandQuery<TChild>(BaseCommandQuery baseCommandQuery) where TChild : BaseCommandQuery, new()
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