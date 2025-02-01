using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCqrsKit(this IServiceCollection services, Action<CqrsKitOptions> options)
    {
        return new CqrsKitBuilder(services).Configure(options);
    }

    internal static void BuildCqrsKit(this IServiceCollection services, List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            CqrsHelper.AddAssembly(assembly);
        }

        var types = CqrsHelper.ConsumerAssemblies.SelectMany(x => x.GetTypes()).ToList();

        services.AddQueryHandlers(types)
                .AddQueryAsyncExecutionFilter(types)
                .AddQueryExecutionFilter(types)
                .AddCommandHandlers(types)
                .AddCommandAsyncExecutionFilter(types)
                .AddCommandExecutionFilter(types)
                .AddApprovalFlowService(types)
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