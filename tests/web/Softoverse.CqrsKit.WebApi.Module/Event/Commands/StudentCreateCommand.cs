using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;
using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Commands;

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