using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.Query;

[ScopedLifetime]
public class QueryExecutionFilter<TQuery, TResponse> : QueryExecutionFilterBase<TQuery, TResponse>
    where TQuery : IQuery
{
    public override Task<Result<TResponse>> OnExecutingAsync(TQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)}");
        return ResultDefaults.DefaultResult<TResponse>();
    }

    public override Task<Result<TResponse>> OnExecutedAsync(TQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)}");
        return ResultDefaults.DefaultResult<TResponse>();
    }
}