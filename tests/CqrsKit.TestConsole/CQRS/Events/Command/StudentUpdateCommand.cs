using System.ComponentModel;

using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Command;
using CqrsKit.TestConsole.Models;

namespace CqrsKit.TestConsole.CQRS.Events.Command;

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