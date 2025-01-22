using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Builders;

public class CqrsKitBuilder
{
    private readonly IServiceCollection _services;

    internal CqrsKitBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection Configure(Action<CqrsKitOptions> configureOptions)
    {
        CqrsKitOptions options = new CqrsKitOptions();
        configureOptions(options);

        if (!options.Assemblies.Any())
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

        _services.BuildCqrsKit(options.Assemblies);

        _services.Configure<CqrsKitOption>(option =>
        {
            option.EnableLogging = options.EnableLogging;
        });

        return _services;
    }
}