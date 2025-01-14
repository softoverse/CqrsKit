using System.ComponentModel;

using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Command;
using CqrsKit.TestConsole.Models;

namespace CqrsKit.TestConsole.CQRS.Events.Command;

[Description("Create student command")]
public class StudentCreateCommand(Student payload) : Command<Student>(payload),
                                                     IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return $"{Payload.Name}_{Payload.Age}_{Payload.Gender}";
    }
}