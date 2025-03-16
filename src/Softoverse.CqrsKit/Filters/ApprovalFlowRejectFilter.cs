using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Filters;

internal class ApprovalFlowRejectFilter : ApprovalFlowRejectFilterBase
{
    public override Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult();

    public override Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult();
}