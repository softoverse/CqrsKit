using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.WebApi.CQRS.Events.Students.Queries;

[Description("Search students query")]
public class StudentGetAllQuery: IQuery
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
}