﻿using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.Query;

[ScopedLifetime]
public class PersonGetAllQueryExecutionFilter : QueryExecutionFilterBase<PersonGetAllQuery, List<Person>>
{
    public override Task<Result<List<Person>>> OnExecutingAsync(PersonGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResultDefaults.DefaultResult<List<Person>>();
    }

    public override Task<Result<List<Person>>> OnExecutedAsync(PersonGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResultDefaults.DefaultResult<List<Person>>();
    }
}