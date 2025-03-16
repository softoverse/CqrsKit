using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Entity;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Executors;

public interface IApprovalFlowExecutor<T> where T : BaseApprovalFlowPendingTask
{
    string ApprovalFlowPendingTaskId { get; init; }

    CqrsContext Context { get; }

    Task<Result> AcceptAsync(CancellationToken ct = default);

    Task<Result> RejectAsync(CancellationToken ct = default);
}