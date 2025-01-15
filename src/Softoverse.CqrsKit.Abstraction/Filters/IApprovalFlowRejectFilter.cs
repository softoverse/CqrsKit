using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IApprovalFlowRejectFilterMarker;
    
public interface IApprovalFlowRejectFilter: IApprovalFlowRejectFilterMarker
{
    Task<Result> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    Task<Result> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}