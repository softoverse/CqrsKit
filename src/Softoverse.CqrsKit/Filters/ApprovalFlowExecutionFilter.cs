using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Filters;

internal class ApprovalFlowExecutionFilter<TCommand, TResponse> : ApprovalFlowExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Result<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();

    public override Task<Result<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();

    public override Task<Result<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();
}