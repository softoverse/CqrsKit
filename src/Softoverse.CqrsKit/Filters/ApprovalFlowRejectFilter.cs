using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

public abstract class ApprovalFlowRejectFilterBase : IApprovalFlowRejectFilter
{
    public abstract Task<Result> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}

internal class ApprovalFlowRejectFilter : ApprovalFlowRejectFilterBase
{
    public override Task<Result> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse();

    public override Task<Result> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse();
}