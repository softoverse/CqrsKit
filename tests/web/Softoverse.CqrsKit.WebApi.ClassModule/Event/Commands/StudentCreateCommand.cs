using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Models.ClassModule;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Commands;

[Description("Create student command")]
public class StudentCreateCommand(Student payload) : Command<Student>(payload),
                                                            IUniqueCommand
{
    public string GetUniqueIdentification()
    {
        return $"{Payload.Name}_{Payload.Age}_{Payload.Gender}";
    }
}