using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

[Description("Delete student command")]
public class PersonDeleteCommand(Guid payload) : Command<Guid>(payload),
                                                  IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return Payload.ToString();
    }
}