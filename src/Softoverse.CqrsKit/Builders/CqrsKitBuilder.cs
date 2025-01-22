using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Builders;

public class CqrsKitBuilder
{
    private readonly IServiceCollection _services;
    private readonly List<Assembly> _assemblies = new();
    private readonly CqrsKitOptions _options = new();

    public CqrsKitBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection Configure(Action<CqrsKitOptions> configureOptions, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        configureOptions(_options);
        
        _services.AddCqrsKit(_options.GetAssemblies())
                 .Build();
        
        // Simplified options configuration
        _services.Configure<CqrsKitOption>(option =>
        {
            option.EnableLogging = _options.EnableLogging;
        });

        return _services;
    }
}