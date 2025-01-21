using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

internal class QueryExecutionFilter<TQuery, TResponse> : QueryExecutionFilterBase<TQuery, TResponse>
    where TQuery : IQuery
{
    public override Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}