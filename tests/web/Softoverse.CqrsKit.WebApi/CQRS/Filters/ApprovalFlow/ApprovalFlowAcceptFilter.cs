using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.WebApi.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowAcceptFilter : ApprovalFlowAcceptFilterBase
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