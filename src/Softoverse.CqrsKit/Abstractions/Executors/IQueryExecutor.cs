using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Executors;

public interface IQueryExecutor<TQuery, TResponse> where TQuery : IQuery
{
    IServiceProvider Services { get; init; }

    TQuery Query { get; init; }
    IQueryHandler<TQuery, TResponse> QueryHandler { get; init; }

    CqrsContext Context { get; }

    Task<Result<TResponse>> ExecuteAsync(CancellationToken ct = default);

    Task<Result<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default);
}