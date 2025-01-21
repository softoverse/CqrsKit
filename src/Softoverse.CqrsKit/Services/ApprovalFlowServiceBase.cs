using Softoverse.CqrsKit.Abstraction.Services;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Entity;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Services;

public class ApprovalFlowServiceBase : IApprovalFlowService
{
    public virtual Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default) => ResponseDefaults.DefaultValueResponse(false);

    public virtual Task<bool> IsApprovalFlowPendingTaskUniqueAsync<TCommand>(TCommand command, CqrsContext context, CancellationToken ct = default)
        where TCommand : ICommand => ResponseDefaults.DefaultValueResponse(true);

    public virtual Task<T> GetApprovalFlowTaskAsync<T>(CqrsContext context, CancellationToken ct = default)
        where T : BaseApprovalFlowPendingTask => ResponseDefaults.DefaultValueResponse(default (T))!;
}