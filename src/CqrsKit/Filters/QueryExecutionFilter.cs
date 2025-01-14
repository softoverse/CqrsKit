using CqrsKit.Abstraction.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Filters;

public abstract class QueryExecutionFilterBase<TQuery, TResponse> : IExecutionFilter<TQuery, TResponse>
    where TQuery : IQuery
{
    public abstract Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public class QueryExecutionFilter<TQuery, TResponse> : QueryExecutionFilterBase<TQuery, TResponse>
    where TQuery : IQuery
{
    public override Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}