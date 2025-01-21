using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IExecutionFilterMarker;

public interface IExecutionFilter<in TRequest, TResponse> : IExecutionFilterMarker
    where TRequest : IRequest
{
    public Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public abstract class CommandExecutionFilterBase<TCommand, TResponse> : IExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
    public abstract Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public abstract class QueryExecutionFilterBase<TQuery, TResponse> : IExecutionFilter<TQuery, TResponse>
    where TQuery : IQuery
{
    public abstract Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}