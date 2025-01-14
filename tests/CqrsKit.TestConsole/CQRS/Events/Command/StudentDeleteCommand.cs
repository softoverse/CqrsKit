using System.ComponentModel;

using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Command;

namespace CqrsKit.TestConsole.CQRS.Events.Command;

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