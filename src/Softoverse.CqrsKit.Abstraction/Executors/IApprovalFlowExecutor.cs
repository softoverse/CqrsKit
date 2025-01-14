using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Entity;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Executors;

public interface IApprovalFlowExecutor<T> where T : BaseApprovalFlowPendingTask
{
    string ApprovalFlowPendingTaskId { get; init; }

    CqrsContext Context { get; }

    Task<Response> AcceptAsync(CancellationToken ct = default);

    Task<Response> RejectAsync(CancellationToken ct = default);
}