using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

internal class ApprovalFlowRejectFilter : ApprovalFlowRejectFilterBase
{
    public override Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult();

    public override Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult();
}