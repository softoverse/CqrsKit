using CqrsKit.Abstraction.Handlers;

using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Executors;

public interface IQueryExecutor<TQuery, TResponse> where TQuery : IQuery
{
    IServiceProvider Services { get; init; }

    TQuery Query { get; init; }
    IQueryHandler<TQuery, TResponse> QueryHandler { get; init; }

    CqrsContext Context { get; }

    Task<Response<TResponse>> ExecuteAsync(CancellationToken ct = default);

    Task<Response<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default);
}