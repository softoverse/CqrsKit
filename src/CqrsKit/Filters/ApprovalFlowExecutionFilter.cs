using CqrsKit.Abstraction.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Filters;

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