using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IExecutionFilterMarker;

public interface IExecutionFilter<in TRequest, TResponse> : IExecutionFilterMarker
    where TRequest : IRequest
{
    public Task<Result<TResponse>> OnExecutingAsync(TRequest request, CqrsContext context, CancellationToken ct = default);

    public Task<Result<TResponse>> OnExecutedAsync(TRequest request, CqrsContext context, CancellationToken ct = default);
}

public abstract class ExecutionFilterBase<TRequest, TResponse> : IExecutionFilter<TRequest, TResponse>
    where TRequest : IRequest
{
    public abstract Task<Result<TResponse>> OnExecutingAsync(TRequest request, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> OnExecutedAsync(TRequest request, CqrsContext context, CancellationToken ct = default);
}

public abstract class CommandExecutionFilterBase<TCommand, TResponse> : ExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
}

public abstract class QueryExecutionFilterBase<TQuery, TResponse> : ExecutionFilterBase<TQuery, TResponse>
    where TQuery : IQuery
{
}