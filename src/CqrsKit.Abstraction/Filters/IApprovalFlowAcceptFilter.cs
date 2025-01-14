using CqrsKit.Model;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Filters;

public interface IApprovalFlowAcceptFilterMarker;
    
public interface IApprovalFlowAcceptFilter: IApprovalFlowAcceptFilterMarker
{
    Task<Response> OnAcceptingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    Task<Response> OnAcceptedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}