using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.WebApi.CQRS.Events.Student.Queries;

[Description("Search students query")]
public class GetAllStudentsQuery: IQuery
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
}