using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.WebApi.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowExecutionFilter<TCommand, TResponse> : ApprovalFlowExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Result<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult<TResponse>();
    }

    public override Task<Result<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult<TResponse>();
    }

    public override Task<Result<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult<TResponse>();
    }
}