using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

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