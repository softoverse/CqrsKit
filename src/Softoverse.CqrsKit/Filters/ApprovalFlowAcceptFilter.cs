using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

public abstract class ApprovalFlowAcceptFilterBase : IApprovalFlowAcceptFilter
{
    public abstract Task<Result> OnAcceptingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result> OnAcceptedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default);
}

internal class ApprovalFlowAcceptFilter : ApprovalFlowAcceptFilterBase
{
    public override Task<Result> OnAcceptingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse();

    public override Task<Result> OnAcceptedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse();
}