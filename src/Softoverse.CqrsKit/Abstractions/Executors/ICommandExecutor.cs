using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Executors;

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