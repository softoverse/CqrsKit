using System.ComponentModel;

using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

[Group("Person")]
[Description("Update person command")]
public class PersonUpdateCommand(Guid id, Person payload) : Command<Person>(payload),
                                                              IUniqueCommand
{
    public Guid Id = id;

    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return Payload.Id.ToString();
    }
}