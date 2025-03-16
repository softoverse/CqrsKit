using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.WebApi.CQRS.Filters.Query;

[ScopedLifetime]
public class QueryExecutionFilter<TQuery, TResponse> : QueryExecutionFilterBase<TQuery, TResponse>
    where TQuery : IQuery
{
    public override Task<Result<TResponse>> OnExecutingAsync(TQuery query, CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult<TResponse>();
    }

    public override Task<Result<TResponse>> OnExecutedAsync(TQuery query, CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult<TResponse>();
    }
}