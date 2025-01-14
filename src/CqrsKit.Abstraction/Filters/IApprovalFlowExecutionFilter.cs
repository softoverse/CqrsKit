using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Filters;

public interface IApprovalFlowExecutionFilterMarker;

public interface IApprovalFlowExecutionFilter<in TCommand, TResponse> : IApprovalFlowExecutionFilterMarker
    where TCommand : ICommand
{
    public Task<Response<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
    
    public Task<Response<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
    
    public Task<Response<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}