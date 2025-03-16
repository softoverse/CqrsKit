using Softoverse.CqrsKit.Abstractions.Handlers.Markers;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Handlers;

public interface IQueryHandler<in TQuery, TResponse> : IQueryHandlerMarker
    where TQuery : IQuery
{
    Task<Result<TResponse>> OnStartAsync(TQuery query, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> HandleAsync(TQuery query, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> OnEndAsync(TQuery query, CqrsContext context, CancellationToken ct = default);
}

public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery
{
    public virtual Task<Result<TResponse>> OnStartAsync(TQuery query, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();

    public abstract Task<Result<TResponse>> HandleAsync(TQuery query, CqrsContext context, CancellationToken ct = default);

    public virtual Task<Result<TResponse>> OnEndAsync(TQuery query, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();
}