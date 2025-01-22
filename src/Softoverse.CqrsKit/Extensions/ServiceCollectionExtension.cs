﻿using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCqrsKit(this IServiceCollection services,
                                                Action<CqrsKitOptions> options,
                                                ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var builder = new CqrsKitBuilder(services);
        return builder.Configure(options, serviceLifetime);
    }

    public static IServiceCollection AddCqrsKit<TMarker>(this IServiceCollection services)
    {
        return services.AddCqrsKit(typeof(TMarker));
    }

    public static IServiceCollection AddCqrsKit(this IServiceCollection services, params List<Type> types)
    {
        return services.AddCqrsKit(types.Select(type => type.Assembly).ToList());
    }
    
    public static IServiceCollection AddCqrsKit(this IServiceCollection services, params List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            CqrsHelper.AddAssembly(assembly);
        }
        return services;
    }

    public static void Build(this IServiceCollection services)
    {
        var types = CqrsHelper.ConsumerAssemblies.SelectMany(x => x.GetTypes()).ToList();
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

    internal static void RegisterToService(IServiceCollection services, Type? interfaceType, Type? implementationType)
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
}