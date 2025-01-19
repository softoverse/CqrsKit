using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;
using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.CQRS.Events.Students.Commands;

[Description("Update student command")]
public class StudentUpdateCommand(Guid id, Student payload) : Command<Student>(payload),
                                                                     IUniqueCommand
{
    public Guid Id = id;

    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return Payload.Id.ToString();
    }
}