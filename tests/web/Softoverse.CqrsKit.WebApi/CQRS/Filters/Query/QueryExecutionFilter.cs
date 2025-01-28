using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

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