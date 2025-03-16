using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Filters;

public interface IApprovalFlowRejectFilterMarker;

public interface IApprovalFlowRejectFilter : IApprovalFlowRejectFilterMarker
{
    Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public abstract class ApprovalFlowRejectFilterBase : IApprovalFlowRejectFilter
{
    public abstract Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}