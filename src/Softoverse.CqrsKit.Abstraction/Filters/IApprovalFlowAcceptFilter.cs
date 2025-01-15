using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IApprovalFlowAcceptFilterMarker;
    
public interface IApprovalFlowAcceptFilter: IApprovalFlowAcceptFilterMarker
{
    Task<Result> OnAcceptingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    Task<Result> OnAcceptedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}