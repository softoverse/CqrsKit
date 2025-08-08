using System.ComponentModel;

using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

[Group("Person")]
[Description("Delete person command")]
public class PersonDeleteCommand(Guid payload) : Command<Guid>(payload),
                                                  IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return Payload.ToString();
    }
}