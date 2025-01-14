using CqrsKit.Model;
using CqrsKit.Model.Entity;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Executors;

public interface IApprovalFlowExecutor<T> where T : BaseApprovalFlowPendingTask
{
    string ApprovalFlowPendingTaskId { get; init; }

    CqrsContext Context { get; }

    Task<Response> AcceptAsync(CancellationToken ct = default);

    Task<Response> RejectAsync(CancellationToken ct = default);
}