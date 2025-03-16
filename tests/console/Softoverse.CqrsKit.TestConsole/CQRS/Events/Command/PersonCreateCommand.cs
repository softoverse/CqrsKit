using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

[Description("Create student command")]
public class PersonCreateCommand(Person payload) : Command<Person>(payload),
                                                     IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return $"{Payload.Name}_{Payload.Age}_{Payload.Gender}";
    }
}