using CqrsKit.Abstraction.Services;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Entity;
using CqrsKit.Model.Utility;

namespace CqrsKit.Services;

public class ApprovalFlowServiceBase : IApprovalFlowService
{
    public virtual Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default) => ResponseDefaults.DefaultValueResponse(false);

    public virtual Task<bool> IsApprovalFlowPendingTaskUniqueAsync<TCommand>(TCommand command, CqrsContext context, CancellationToken ct = default)
        where TCommand : ICommand => ResponseDefaults.DefaultValueResponse(true);

    public virtual Task<T?> GetApprovalFlowTaskAsync<T>(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
        where T : BaseApprovalFlowPendingTask => ResponseDefaults.DefaultValueResponse(default (T));
}