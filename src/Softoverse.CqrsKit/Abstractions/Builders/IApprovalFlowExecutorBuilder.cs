using Softoverse.CqrsKit.Abstractions.Executors;
using Softoverse.CqrsKit.Models.Entity;

namespace Softoverse.CqrsKit.Abstractions.Builders;

public interface IApprovalFlowExecutorBuilder<T> where T : BaseApprovalFlowPendingTask
{
    IApprovalFlowExecutorBuilder<T> WithId(string id);

    IApprovalFlowExecutorBuilder<T> WithItems(IDictionary<object, object?> items);

    IApprovalFlowExecutor<T> Build();
}