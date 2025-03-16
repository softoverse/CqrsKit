using Softoverse.CqrsKit.Abstractions.Handlers.Markers;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Handlers;

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandlerMarker
    where TCommand : ICommand
{
    Task<Result<TResponse>> ValidateAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> OnStartAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> HandleAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> OnEndAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}

public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand
{
    public virtual Task<Result<TResponse>> ValidateAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();

    public virtual Task<Result<TResponse>> OnStartAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();

    public abstract Task<Result<TResponse>> HandleAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    public virtual Task<Result<TResponse>> OnEndAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResultDefaults.DefaultResult<TResponse>();
}