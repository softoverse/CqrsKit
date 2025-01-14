using CqrsKit.Abstraction.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Utility;

namespace CqrsKit.Filters;

public abstract class ApprovalFlowRejectFilterBase : IApprovalFlowRejectFilter
{
    public abstract Task<Response> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Response> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}

public class ApprovalFlowRejectFilter : ApprovalFlowRejectFilterBase
{
    public override Task<Response> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse();

    public override Task<Response> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse();
}