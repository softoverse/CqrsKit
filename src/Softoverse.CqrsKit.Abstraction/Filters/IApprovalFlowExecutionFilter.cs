using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IApprovalFlowExecutionFilterMarker;

public interface IApprovalFlowExecutionFilter<in TCommand, TResponse> : IApprovalFlowExecutionFilterMarker
    where TCommand : ICommand
{
    public Task<Response<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
    
    public Task<Response<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
    
    public Task<Response<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}