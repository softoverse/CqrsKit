using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IApprovalFlowAcceptFilterMarker;
    
public interface IApprovalFlowAcceptFilter: IApprovalFlowAcceptFilterMarker
{
    Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public abstract class ApprovalFlowAcceptFilterBase : IApprovalFlowAcceptFilter
{
    public abstract Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}