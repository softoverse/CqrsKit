using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;

[Description("Get person by Id query")]
public class PersonGetByIdQuery : IQuery
{
    public Guid Id { get; set; }
}