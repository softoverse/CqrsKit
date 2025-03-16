using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Queries;

[Description("Get student by Id query")]
public class StudentGetByIdQuery : IQuery
{
    public Guid Id { get; set; }
}