using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Entity;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Services;

public interface IApprovalFlowService
{
    public Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default);

    public Task<bool> IsApprovalFlowPendingTaskUniqueAsync<TCommand>(TCommand command, CqrsContext context, CancellationToken ct = default)
        where TCommand : ICommand;

    public Task<T?> GetApprovalFlowTaskAsync<T>(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
        where T : BaseApprovalFlowPendingTask;
}