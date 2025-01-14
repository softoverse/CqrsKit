using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IApprovalFlowRejectFilterMarker;
    
public interface IApprovalFlowRejectFilter: IApprovalFlowRejectFilterMarker
{
    Task<Response> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    Task<Response> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}