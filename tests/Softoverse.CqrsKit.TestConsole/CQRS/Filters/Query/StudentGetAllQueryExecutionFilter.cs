﻿using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.Query;

[ScopedLifetime]
public class StudentGetAllQueryExecutionFilter : QueryExecutionFilterBase<StudentGetAllQuery, List<Student>>
{
    public override Task<Result<List<Student>>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<List<Student>>();
    }

    public override Task<Result<List<Student>>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<List<Student>>();
    }
}