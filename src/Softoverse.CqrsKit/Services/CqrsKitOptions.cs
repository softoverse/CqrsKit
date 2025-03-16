using System.Reflection;

using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Services;

public class CqrsKitOptions : CqrsKitOption
{
    private readonly List<Assembly> _assemblies = new();

    public CqrsKitOptions RegisterServicesFromAssemblyContaining<TMarker>()
    {
        _assemblies.Add(typeof(TMarker).Assembly);
        return this;
    }

    public CqrsKitOptions RegisterServicesFromAssemblyContaining(Type type)
    {
        _assemblies.Add(type.Assembly);
        return this;
    }

    public List<Assembly> Assemblies
    {
        get => _assemblies;
    }
}