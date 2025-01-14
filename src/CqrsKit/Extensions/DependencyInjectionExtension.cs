using System.Reflection;

using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Services;

using CqrsKit.Abstraction.Filters;
using CqrsKit.Abstraction.Handlers;
using CqrsKit.Abstraction.Handlers.Markers;
using CqrsKit.Abstraction.Services;

using CqrsKit.Model.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace CqrsKit.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddCqrsKit<TMarker>(this IServiceCollection services)
    {
        Assembly assembly = typeof(TMarker).Assembly;

        CqrsHelper.AddAssembly(assembly);

        return services.AddApprovalFlowService(assembly)
                      .AddCommandHandlers(assembly)
                      .AddQueryHandlers(assembly)
                      .AddCommandExecutorFilter(assembly)
                      .AddQueryExecutorFilter(assembly)
                      .AddApprovalFlowExecutorFilter(assembly)
                      .AddApprovalFlowHandlers(assembly)
                      .AddApprovalFlowAcceptFilter(assembly)
                      .AddApprovalFlowRejectFilter(assembly);
    }

    #region Command Handlers

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        foreach (var implementationType in assembly.GetCommandHandlerTypes())
        {
            var interfaceType = implementationType.GetInterfaces()
                                                  .FirstOrDefault(type => typeof(ICommandHandlerMarker)
                                                                              .IsAssignableFrom(type)
                                                                        &&
                                                                          type != typeof(ICommandHandlerMarker));

            RegisterToService(services, interfaceType, implementationType);
            RegisterToService(services, implementationType);
        }

        return services;
    }

    #endregion Command Handlers

    #region Approval Flow Handlers

    private static IServiceCollection AddApprovalFlowHandlers(this IServiceCollection services, Assembly assembly)
    {
        var implementationTypes = assembly.GetTypes()
                                          .Where(type =>
                                                     type.GetTypeInfo().IsClass
                                                   &&
                                                     typeof(IApprovalFlowHandlerMarker).IsAssignableFrom(type)
                                                   &&
                                                     type != typeof(IApprovalFlowHandlerMarker));

        foreach (var implementationType in implementationTypes)
        {
            var interfaceType = implementationType.GetInterfaces().FirstOrDefault(type => typeof(IApprovalFlowHandlerMarker).IsAssignableFrom(type) && type != typeof(IApprovalFlowHandlerMarker));

            RegisterToService(services, interfaceType, implementationType);
            RegisterToService(services, implementationType);
        }

        return services;
    }

    #endregion Approval Flow Handlers

    #region CQRS Configuration

    private static IServiceCollection AddApprovalFlowService(this IServiceCollection services, Assembly assembly)
    {
        var implementationType = assembly.GetTypes()
                                         .FirstOrDefault(type =>
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

    private static IServiceCollection AddCommandExecutorFilter(this IServiceCollection services, Assembly assembly)
    {
        var commandHandlerTypes = assembly.GetCommandHandlerTypes();

        Type implementationType = assembly.GetTypes()
                                          .FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                                &&
                                                                  typeof(IExecutionFilterMarker).IsAssignableFrom(type)
                                                                &&
                                                                  type != typeof(IExecutionFilterMarker)
                                                               && typeof(ICommand).IsAssignableFrom(type.GetGenericArguments()[0]))
                               ?? typeof(CommandExecutionFilter<,>);

        Type interfaceType = typeof(IExecutionFilter<,>);

        foreach (var commandHandlerType in commandHandlerTypes)
        {
            Type[] commandHandlerGenericTypes = commandHandlerType.GetInterface(typeof(ICommandHandler<,>).Name)?.GetGenericArguments() ?? [];
            var newImplementationType = implementationType.MakeGenericType([..commandHandlerGenericTypes]);
            var newInterfaceType = interfaceType.MakeGenericType([..commandHandlerGenericTypes]);
            RegisterToService(services, newInterfaceType, newImplementationType);
        }

        return services;
    }

    private static IServiceCollection AddQueryExecutorFilter(this IServiceCollection services, Assembly assembly)
    {
        var queryHandlerTypes = assembly.GetQueryHandlerTypes();


        var implementationType = assembly.GetTypes()
                                         .FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                               &&
                                                                 typeof(IExecutionFilterMarker).IsAssignableFrom(type)
                                                               &&
                                                                 type != typeof(IExecutionFilterMarker)
                                                              && typeof(IQuery).IsAssignableFrom(type.GetGenericArguments()[0]))
                              ?? typeof(QueryExecutionFilter<,>);

        Type interfaceType = typeof(IExecutionFilter<,>);

        foreach (var queryHandlerType in queryHandlerTypes)
        {
            Type[] queryHandlerGenericTypes = queryHandlerType.GetInterface(typeof(IQueryHandler<,>).Name)?.GetGenericArguments() ?? [];
            var newImplementationType = implementationType?.MakeGenericType([..queryHandlerGenericTypes]);
            var newInterfaceType = interfaceType?.MakeGenericType([..queryHandlerGenericTypes]);
            RegisterToService(services, newInterfaceType, newImplementationType);
        }

        return services;
    }

    private static IServiceCollection AddApprovalFlowExecutorFilter(this IServiceCollection services, Assembly assembly)
    {
        var commandHandlerTypes = assembly.GetCommandHandlerTypes();

        Type implementationType = assembly.GetTypes()
                                          .FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                                &&
                                                                  typeof(IApprovalFlowExecutionFilterMarker).IsAssignableFrom(type)
                                                                &&
                                                                  type != typeof(IApprovalFlowExecutionFilterMarker)
                                                               && typeof(ICommand).IsAssignableFrom(type.GetGenericArguments()[0]))
                               ?? typeof(ApprovalFlowExecutionFilter<,>);

        Type interfaceType = typeof(IApprovalFlowExecutionFilter<,>);

        foreach (var commandHandlerType in commandHandlerTypes)
        {
            Type[] commandHandlerGenericTypes = commandHandlerType.GetInterface(typeof(ICommandHandler<,>).Name)?.GetGenericArguments() ?? [];
            var newImplementationType = implementationType.MakeGenericType([..commandHandlerGenericTypes]);
            var newInterfaceType = interfaceType.MakeGenericType([..commandHandlerGenericTypes]);
            RegisterToService(services, newInterfaceType, newImplementationType);
        }

        return services;
    }

    private static IServiceCollection AddApprovalFlowAcceptFilter(this IServiceCollection services, Assembly assembly)
    {
        Type implementationType = assembly.GetTypes()
                                          .FirstOrDefault(type => type.GetTypeInfo().IsClass
                                                                &&
                                                                  typeof(IApprovalFlowAcceptFilterMarker).IsAssignableFrom(type)
                                                                &&
                                                                  type != typeof(IApprovalFlowAcceptFilterMarker))
                               ?? typeof(ApprovalFlowAcceptFilter);

        Type interfaceType = typeof(IApprovalFlowAcceptFilter);

        RegisterToService(services, interfaceType, implementationType);

        return services;
    }

    private static IServiceCollection AddApprovalFlowRejectFilter(this IServiceCollection services, Assembly assembly)
    {
        Type implementationType = assembly.GetTypes()
                                          .FirstOrDefault(type => type.GetTypeInfo().IsClass
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

    #region Query Handlers

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        foreach (var implementationType in assembly.GetQueryHandlerTypes())
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

    // [Obsolete("Use other overload instead", true)]
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

    private static IEnumerable<Type> GetQueryHandlerTypes(this Assembly assembly)
    {
        return assembly.GetTypes()
                       .Where(type =>
                                  type.GetTypeInfo().IsClass
                                &&
                                  typeof(IQueryHandlerMarker).IsAssignableFrom(type)
                                &&
                                  type != typeof(IQueryHandlerMarker));
    }

    private static IEnumerable<Type> GetCommandHandlerTypes(this Assembly assembly)
    {
        return assembly.GetTypes()
                       .Where(type =>
                                  type.GetTypeInfo().IsClass
                                &&
                                  typeof(ICommandHandlerMarker).IsAssignableFrom(type)
                                &&
                                  type != typeof(ICommandHandlerMarker));
    }
}