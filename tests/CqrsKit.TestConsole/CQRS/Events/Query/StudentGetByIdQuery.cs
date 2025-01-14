using System.ComponentModel;

using CqrsKit.Model.Abstraction;

namespace CqrsKit.TestConsole.CQRS.Events.Query;

[Description("Get student by Id query")]
public class StudentGetByIdQuery : IQuery
{
    public Guid Id { get; set; }
}