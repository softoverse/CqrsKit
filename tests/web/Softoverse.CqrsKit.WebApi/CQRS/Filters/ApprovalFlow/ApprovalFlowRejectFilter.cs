using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.WebApi.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowRejectFilter : ApprovalFlowRejectFilterBase
{
    public override Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult();
    }

    public override Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return ResultDefaults.DefaultResult();
    }
}