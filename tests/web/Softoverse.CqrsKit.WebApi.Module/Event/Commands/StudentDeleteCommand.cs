using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;

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