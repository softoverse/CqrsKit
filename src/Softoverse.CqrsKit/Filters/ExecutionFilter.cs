using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Filters;

internal class ExecutionFilter<TRequest, TResponse> : ExecutionFilterBase<TRequest, TResponse>
    where TRequest : IRequest
{
    public override Task<Result<TResponse>> OnExecutingAsync(TRequest request, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();

    public override Task<Result<TResponse>> OnExecutedAsync(TRequest request, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();
}

internal class CommandExecutionFilter<TCommand, TResponse> : ExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
}

internal class QueryExecutionFilter<TQuery, TResponse> : ExecutionFilter<TQuery, TResponse>
    where TQuery : IQuery
{
}