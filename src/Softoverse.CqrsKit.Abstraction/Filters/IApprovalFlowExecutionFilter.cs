using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IApprovalFlowExecutionFilterMarker;

public interface IApprovalFlowExecutionFilter<in TCommand, TResponse> : IApprovalFlowExecutionFilterMarker
    where TCommand : ICommand
{
    public Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);
    
    public Task<Result<TResponse>> ExecuteAsync(CqrsContext context, CancellationToken ct = default);
    
    public Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public abstract class ApprovalFlowExecutionFilterBase<TCommand, TResponse> : IApprovalFlowExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
    public abstract Task<Result<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> ExecuteAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}