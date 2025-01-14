using System.ComponentModel;

using CqrsKit.Model.Abstraction;

namespace CqrsKit.TestConsole.CQRS.Events.Query;

[Description("Search students query")]
public class StudentGetAllQuery : IQuery
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
}