using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Command;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Commands;

[Description("Delete student command")]
public class StudentDeleteCommand(Guid payload) : Command<Guid>(payload),
                                                  IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        return Payload.ToString();
    }
}