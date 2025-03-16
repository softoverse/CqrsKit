namespace Softoverse.CqrsKit.Abstractions.Executors;

/// <summary>
/// Should be used only in case of Approval Flow
/// </summary>
public interface ICommandApprovalFlowEventExecutor
{
    /// <summary>
    /// Should be used only in case of Approval Flow
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<object> ExecuteDynamicAsync(CancellationToken ct = default);

    Task<object> AfterAcceptAsync(CancellationToken ct = default);

    Task<object> AfterRejectAsync(CancellationToken ct = default);
}