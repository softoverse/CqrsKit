using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;

[Description("Search person query")]
public class PersonGetAllQuery : IQuery
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
}