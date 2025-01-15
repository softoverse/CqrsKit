using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Entity;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Executors;

public interface IApprovalFlowExecutor<T> where T : BaseApprovalFlowPendingTask
{
    string ApprovalFlowPendingTaskId { get; init; }

    CqrsContext Context { get; }

    Task<Result> AcceptAsync(CancellationToken ct = default);

    Task<Result> RejectAsync(CancellationToken ct = default);
}