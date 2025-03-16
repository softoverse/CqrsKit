using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Filters;

public interface IApprovalFlowExecutionFilterMarker;

public interface IApprovalFlowExecutionFilter<in TCommand, TResponse> : IApprovalFlowExecutionFilterMarker
    where TCommand : ICommand
{
    public Task<Result<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
    
    public Task<Result<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
    
    public Task<Result<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}

public abstract class ApprovalFlowExecutionFilterBase<TCommand, TResponse> : IApprovalFlowExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
    public abstract Task<Result<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}