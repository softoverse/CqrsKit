using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Entity;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Services;

public interface IApprovalFlowService
{
    public Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default);

    public Task<bool> IsApprovalFlowPendingTaskUniqueAsync<TCommand>(TCommand command, CqrsContext context, CancellationToken ct = default)
        where TCommand : ICommand;

    public Task<T> GetApprovalFlowTaskAsync<T>(CqrsContext context, CancellationToken ct = default)
        where T : BaseApprovalFlowPendingTask;
}