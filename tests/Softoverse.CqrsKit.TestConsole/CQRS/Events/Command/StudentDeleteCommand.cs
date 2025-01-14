using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

[Description("Delete student command")]
public class StudentDeleteCommand(Guid payload) : Command<Guid>(payload),
                                                  IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetUniqueIdentification)}");
        return Payload.ToString();
    }
}