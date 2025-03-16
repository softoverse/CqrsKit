using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Command;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Models.ClassModule;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Commands;

[Description("Update student command")]
public class StudentUpdateCommand(Guid id, Student payload) : Command<Student>(payload),
                                                                     IUniqueCommand
{
    public Guid Id = id;

    public string GetUniqueIdentification()
    {
        return Payload.Id.ToString();
    }
}