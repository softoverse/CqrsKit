using CqrsKit.Abstraction.Handlers;

using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Executors;

public interface ICommandExecutor<TCommand, TResponse> where TCommand : ICommand
{
    IServiceProvider Services { get; init; }

    bool IsApprovalFlowEnabled { get; init; }

    CqrsContext Context { get; }

    TCommand Command { get; init; }

    ICommandHandler<TCommand, TResponse> CommandHandler { get; init; }
    IApprovalFlowHandler<TCommand, TResponse> ApprovalFlowHandler { get; init; }

    Task<Response<TResponse>> ExecuteAsync(CancellationToken ct = default);

    Task<Response<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default);
}