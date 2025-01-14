using CqrsKit.Abstraction.Executors;

using CqrsKit.Model.Entity;

namespace CqrsKit.Abstraction.Builders;

public interface IApprovalFlowExecutorBuilder<T> where T : BaseApprovalFlowPendingTask
{
    IApprovalFlowExecutorBuilder<T> WithId(string id);

    IApprovalFlowExecutorBuilder<T> WithItems(IDictionary<object, object?> items);

    IApprovalFlowExecutor<T> Build();
}