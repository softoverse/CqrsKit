﻿using System.Reflection;

using Microsoft.AspNetCore.Builder;

using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Abstraction.Handlers.Markers;
using Softoverse.CqrsKit.Abstraction.Services;
using Softoverse.CqrsKit.Model.Abstraction;

using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddCqrsKit<TMarker>(this IServiceCollection services)
    {
        return services.AddCqrsKit(typeof(TMarker));
    }

    public static IServiceCollection AddCqrsKit(this IServiceCollection services, params List<Type> types)
    {
        foreach (var assembly in types.Select(type => type.Assembly))
        {
            CqrsHelper.AddAssembly(assembly);
        }
        return services;
    }

    public static void Build(this IServiceCollection services)
    {
        var types = CqrsHelper.AddedAssemblies.SelectMany(x => x.GetTypes()).ToList();
        services.AddApprovalFlowService(types)
                .AddQueryHandlers(types)
                .AddQueryExecutionFilter(types)
                .AddCommandHandlers(types)
                .AddCommandExecutionFilter(types)
                .AddApprovalFlowExecutionFilter(types)
                .AddApprovalFlowHandlers(types)
                .AddApprovalFlowAcceptFilter(types)
                .AddApprovalFlowRejectFilter(types);
    }

    #region Query Handlers

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services, IList<Type> types)
    {
        foreach (var implementationType in types.GetQueryHandlerTypes())
        {
            var interfaceType = implementationType.GetInterfaces()
                                                  .FirstOrDefault(type =>
                                                                      typeof(IQueryHandlerMarker).IsAssignableFrom(type)
                                                                    &&
                                                                      type != typeof(IQueryHandlerMarker));

            RegisterToService(services, interfaceType, implementationType);
            // RegisterToService(services, implementationType);
        }

        return services;
    }

    #endregion Query Handlers

    #region Command Handlers

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services, IList<Type> types)
    {
        foreach (var implementationType in types.GetCommandHandlerTypes())
        {
            var interfaceType = implementationType.GetInterfaces()
                                                  .FirstOrDefault(type => typeof(ICommandHandlerMarker)
                                                                              .IsAssignableFrom(type)
                                                                        &&
                                                                          type != typeof(ICommandHandlerMarker));

            RegisterToService(services, interfaceType, implementationType);
            // RegisterToService(services, implementationType);
        }

        return services;
    }

    #endregion Command Handlers

    #region Approval Flow Handlers

    private static IServiceCollection AddApprovalFlowHandlers(this IServiceCollection services, IList<Type> types)
    {
        var implementationTypes = types.Where(type =>
                                                  type.GetTypeInfo().IsClass
                                                &&
                                                  typeof(IApprovalFlowHandlerMarker).IsAssignableFrom(type)
                                                &&
                                                  type != typeof(IApprovalFlowHandlerMarker));

        foreach (var implementationType in implementationTypes)
        {
            var interfaceType = implementationType.GetInterfaces().FirstOrDefault(type => typeof(IApprovalFlowHandlerMarker).IsAssignableFrom(type) && type != typeof(IApprovalFlowHandlerMarker));

            RegisterToService(services, interfaceType, implementationType);
            // RegisterToService(services, implementationType);
        }

        return services;
    }

    #endregion Approval Flow Handlers

    #region CQRS Configuration

    private static IServiceCollection AddQueryExecutionFilter(this IServiceCollection services, IList<Type> types)
    {
        var queryHandlerTypes = types.GetQueryHandlerTypes();


        var implementationType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                            &&
                                                              typeof(IExecutionFilterMarker).IsAssignableFrom(type)
                                                            &&
                                                              type != typeof(IExecutionFilterMarker)
                                                           && typeof(IQuery).IsAssignableFrom(type.GetGenericArguments().FirstOrDefault()))
                              ?? typeof(QueryExecutionFilter<,>);

        Type interfaceType = typeof(IExecutionFilter<,>);

        foreach (var queryHandlerType in queryHandlerTypes)
        {
            Type[] queryHandlerGenericTypes = queryHandlerType.GetInterface(typeof(IQueryHandler<,>).Name)?.GetGenericArguments() ?? [];

            var newQueryExecutionFilterBaseType = typeof(QueryExecutionFilterBase<,>).MakeGenericType(queryHandlerGenericTypes);
            var customQueryExecutionFilterType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                                            &&
                                                                              newQueryExecutionFilterBaseType.IsAssignableFrom(type)
                                                                            &&
                                                                              type != newQueryExecutionFilterBaseType);

            var newInterfaceType = interfaceType?.MakeGenericType(queryHandlerGenericTypes);
            if (customQueryExecutionFilterType != null)
            {
                var newImplementationType = customQueryExecutionFilterType;
                RegisterToService(services, newInterfaceType, newImplementationType);
            }
            else
            {
                var newImplementationType = implementationType?.MakeGenericType(queryHandlerGenericTypes);
                RegisterToService(services, newInterfaceType, newImplementationType);
            }
        }

        return services;
    }

    private static IServiceCollection AddCommandExecutionFilter(this IServiceCollection services, IList<Type> types)
    {
        var commandHandlerTypes = types.GetCommandHandlerTypes();

        Type implementationType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                             &&
                                                               typeof(IExecutionFilterMarker).IsAssignableFrom(type)
                                                             &&
                                                               type != typeof(IExecutionFilterMarker)
                                                            && typeof(ICommand).IsAssignableFrom(type.GetGenericArguments().FirstOrDefault()))
                               ?? typeof(CommandExecutionFilter<,>);

        Type interfaceType = typeof(IExecutionFilter<,>);

        foreach (var commandHandlerType in commandHandlerTypes)
        {
            Type[] commandHandlerGenericTypes = commandHandlerType.GetInterface(typeof(ICommandHandler<,>).Name)?.GetGenericArguments() ?? [];

            var newCommandExecutionFilterBaseType = typeof(CommandExecutionFilterBase<,>).MakeGenericType(commandHandlerGenericTypes);
            var customCommandExecutionFilterType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                                              &&
                                                                                newCommandExecutionFilterBaseType.IsAssignableFrom(type)
                                                                              &&
                                                                                type != newCommandExecutionFilterBaseType);

            var newInterfaceType = interfaceType?.MakeGenericType(commandHandlerGenericTypes);
            if (customCommandExecutionFilterType != null)
            {
                var newImplementationType = customCommandExecutionFilterType;
                RegisterToService(services, newInterfaceType, newImplementationType);
            }
            else
            {
                var newImplementationType = implementationType?.MakeGenericType(commandHandlerGenericTypes);
                RegisterToService(services, newInterfaceType, newImplementationType);
            }
        }

        return services;
    }

    private static IServiceCollection AddApprovalFlowService(this IServiceCollection services, IList<Type> types)
    {
        var implementationType = types.FirstOrDefault(type =>
                                                          type.GetTypeInfo().IsClass
                                                        &&
                                                          typeof(IApprovalFlowService).IsAssignableFrom(type)
                                                        &&
                                                          type != typeof(IApprovalFlowService))
                              ?? typeof(ApprovalFlowServiceBase);

        if (implementationType is null)
        {
            throw new NotImplementedException($"The class '{nameof (ApprovalFlowServiceBase)}' is not implemented.");
        }

        Type? interfaceType = implementationType.GetInterfaces().FirstOrDefault(type => typeof(IApprovalFlowService).IsAssignableFrom(type));

        RegisterToService(services, interfaceType, implementationType);
        // RegisterToService(services, implementationType);

        return services;
    }

    private static IServiceCollection AddApprovalFlowExecutionFilter(this IServiceCollection services, IList<Type> types)
    {
        var commandHandlerTypes = types.GetCommandHandlerTypes();

        Type implementationType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                             &&
                                                               typeof(IApprovalFlowExecutionFilterMarker).IsAssignableFrom(type)
                                                             &&
                                                               type != typeof(IApprovalFlowExecutionFilterMarker)
                                                            && typeof(ICommand).IsAssignableFrom(type.GetGenericArguments().FirstOrDefault()))
                               ?? typeof(ApprovalFlowExecutionFilter<,>);

        Type interfaceType = typeof(IApprovalFlowExecutionFilter<,>);

        foreach (var commandHandlerType in commandHandlerTypes)
        {
            Type[] commandHandlerGenericTypes = commandHandlerType.GetInterface(typeof(ICommandHandler<,>).Name)?.GetGenericArguments() ?? [];

            var newApprovalFlowExecutionFilterBaseType = typeof(ApprovalFlowExecutionFilterBase<,>).MakeGenericType(commandHandlerGenericTypes);
            var customApprovalFlowExecutionFilterType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                                                   &&
                                                                                     newApprovalFlowExecutionFilterBaseType.IsAssignableFrom(type)
                                                                                   &&
                                                                                     type != newApprovalFlowExecutionFilterBaseType);

            var newInterfaceType = interfaceType?.MakeGenericType(commandHandlerGenericTypes);
            if (customApprovalFlowExecutionFilterType != null)
            {
                var newImplementationType = customApprovalFlowExecutionFilterType;
                RegisterToService(services, newInterfaceType, newImplementationType);
            }
            else
            {
                var newImplementationType = implementationType?.MakeGenericType(commandHandlerGenericTypes);
                RegisterToService(services, newInterfaceType, newImplementationType);
            }
        }

        return services;
    }

    private static IServiceCollection AddApprovalFlowAcceptFilter(this IServiceCollection services, IList<Type> types)
    {
        Type implementationType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                             &&
                                                               typeof(IApprovalFlowAcceptFilterMarker).IsAssignableFrom(type)
                                                             &&
                                                               type != typeof(IApprovalFlowAcceptFilterMarker))
                               ?? typeof(ApprovalFlowAcceptFilter);

        Type interfaceType = typeof(IApprovalFlowAcceptFilter);

        RegisterToService(services, interfaceType, implementationType);

        return services;
    }

    private static IServiceCollection AddApprovalFlowRejectFilter(this IServiceCollection services, IList<Type> types)
    {
        Type implementationType = types.FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                             &&
                                                               typeof(IApprovalFlowRejectFilterMarker).IsAssignableFrom(type)
                                                             &&
                                                               type != typeof(IApprovalFlowRejectFilterMarker))
                               ?? typeof(ApprovalFlowRejectFilter);

        Type interfaceType = typeof(IApprovalFlowRejectFilter);

        RegisterToService(services, interfaceType, implementationType);

        return services;
    }

    #endregion CQRS Configuration

    private static void RegisterToService(IServiceCollection services, Type? interfaceType, Type? implementationType)
    {
        if (interfaceType is null || implementationType is null)
        {
            throw new NotImplementedException("The interface or class is not implemented.");
        }

        if (implementationType.GetCustomAttribute<TransientLifetimeAttribute>() is not null) services.AddTransient(interfaceType, implementationType);
        else if (implementationType.GetCustomAttribute<ScopedLifetimeAttribute>() is not null) services.AddScoped(interfaceType, implementationType);
        else if (implementationType.GetCustomAttribute<SingletonLifetimeAttribute>() is not null) services.AddSingleton(interfaceType, implementationType);
        else services.AddScoped(interfaceType, implementationType);
    }

    [Obsolete("Use other overload instead", true)]
    private static void RegisterToService(IServiceCollection services, Type? implementationType)
    {
        if (implementationType is null)
        {
            throw new NotImplementedException($"The class is not implemented.");
        }

        if (implementationType.GetCustomAttribute<TransientLifetimeAttribute>() is not null) services.AddTransient(implementationType);
        else if (implementationType.GetCustomAttribute<ScopedLifetimeAttribute>() is not null) services.AddScoped(implementationType);
        else if (implementationType.GetCustomAttribute<SingletonLifetimeAttribute>() is not null) services.AddSingleton(implementationType);
        else services.AddScoped(implementationType);
    }

    private static IEnumerable<Type> GetQueryHandlerTypes(this IList<Type> types)
    {
        return types.Where(type =>
                               type.GetTypeInfo().IsClass
                             &&
                               typeof(IQueryHandlerMarker).IsAssignableFrom(type)
                             &&
                               type != typeof(IQueryHandlerMarker));
    }

    private static IEnumerable<Type> GetCommandHandlerTypes(this IList<Type> types)
    {
        return types.Where(type =>
                               type.GetTypeInfo().IsClass
                             &&
                               typeof(ICommandHandlerMarker).IsAssignableFrom(type)
                             &&
                               type != typeof(ICommandHandlerMarker));
    }
}