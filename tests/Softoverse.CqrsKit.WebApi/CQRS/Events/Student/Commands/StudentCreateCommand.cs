using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;

namespace Softoverse.CqrsKit.WebApi.CQRS.Events.Student.Commands;

[Description("Create student command")]
public class StudentCreateCommand(Models.Student payload) : Command<Models.Student>(payload),
                                                     IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return $"{Payload.Name}_{Payload.Age}_{Payload.Gender}";
    }
}