using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

internal class ExecutionFilter<TRequest, TResponse> : ExecutionFilterBase<TRequest, TResponse>
    where TRequest : IRequest
{
    public override Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}

internal class CommandExecutionFilter<TCommand, TResponse> : ExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
}

internal class QueryExecutionFilter<TQuery, TResponse> : ExecutionFilter<TQuery, TResponse>
    where TQuery : IQuery
{
}