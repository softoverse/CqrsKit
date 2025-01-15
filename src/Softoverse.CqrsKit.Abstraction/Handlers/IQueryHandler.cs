using Softoverse.CqrsKit.Abstraction.Handlers.Markers;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Handlers;

public interface IQueryHandler<in TQuery, TResponse> : IQueryHandlerMarker
    where TQuery : IQuery
{
    Task<Result<TResponse>> OnStartAsync(TQuery command, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> HandleAsync(TQuery command, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> OnEndAsync(TQuery command, CqrsContext context, CancellationToken ct = default);
}

public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery
{
    public virtual Task<Result<TResponse>> OnStartAsync(TQuery command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public abstract Task<Result<TResponse>> HandleAsync(TQuery command, CqrsContext context, CancellationToken ct = default);

    public virtual Task<Result<TResponse>> OnEndAsync(TQuery command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}