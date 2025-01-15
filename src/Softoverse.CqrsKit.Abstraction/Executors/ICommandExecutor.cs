using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Executors;

public interface ICommandExecutor<TCommand, TResponse> where TCommand : ICommand
{
    IServiceProvider Services { get; init; }

    bool IsApprovalFlowEnabled { get; init; }

    CqrsContext Context { get; }

    TCommand Command { get; init; }

    ICommandHandler<TCommand, TResponse> CommandHandler { get; init; }
    IApprovalFlowHandler<TCommand, TResponse> ApprovalFlowHandler { get; init; }

    Task<Result<TResponse>> ExecuteAsync(CancellationToken ct = default);

    Task<Result<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default);
}