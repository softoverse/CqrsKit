﻿using System.ComponentModel;

using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.WebApi.Module.Event.Queries;

[Description("Search students query")]
public class StudentGetAllQuery: IQuery
{
    public string? Name { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
}