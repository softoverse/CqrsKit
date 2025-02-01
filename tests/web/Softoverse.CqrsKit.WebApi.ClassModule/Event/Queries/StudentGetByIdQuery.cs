using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Queries;

[Description("Get student by Id query")]
public class StudentGetByIdQuery : IQuery
{
    public Guid Id { get; set; }
}