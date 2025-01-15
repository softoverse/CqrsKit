using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Executors;

public interface IQueryExecutor<TQuery, TResponse> where TQuery : IQuery
{
    IServiceProvider Services { get; init; }

    TQuery Query { get; init; }
    IQueryHandler<TQuery, TResponse> QueryHandler { get; init; }

    CqrsContext Context { get; }

    Task<Result<TResponse>> ExecuteAsync(CancellationToken ct = default);

    Task<Result<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default);
}