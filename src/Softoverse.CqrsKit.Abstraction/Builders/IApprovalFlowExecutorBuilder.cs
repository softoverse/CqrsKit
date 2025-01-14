using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Model.Entity;

namespace Softoverse.CqrsKit.Abstraction.Builders;

public interface IApprovalFlowExecutorBuilder<T> where T : BaseApprovalFlowPendingTask
{
    IApprovalFlowExecutorBuilder<T> WithId(string id);

    IApprovalFlowExecutorBuilder<T> WithItems(IDictionary<object, object?> items);

    IApprovalFlowExecutor<T> Build();
}