using System.ComponentModel;

using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;

[Group("Person")]
[Description("Get person by Id query")]
public class PersonGetByIdQuery : IQuery
{
    public Guid Id { get; set; }
}