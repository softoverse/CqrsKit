﻿using System.ComponentModel;

using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Command;
using Softoverse.CqrsKit.WebApi.Models;

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