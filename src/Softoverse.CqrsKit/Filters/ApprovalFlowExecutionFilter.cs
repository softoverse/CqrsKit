using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

public abstract class ApprovalFlowExecutionFilterBase<TCommand, TResponse> : IApprovalFlowExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
    public abstract Task<Response<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Response<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Response<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}

internal class ApprovalFlowExecutionFilter<TCommand, TResponse> : ApprovalFlowExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Response<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Response<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Response<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}