using Softoverse.CqrsKit.Abstraction.Handlers.Markers;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Handlers;

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandlerMarker
    where TCommand : ICommand
{
    Task<Result<TResponse>> ValidateAsync(CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> OnStartAsync(CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> HandleAsync(CqrsContext context, CancellationToken ct = default);

    Task<Result<TResponse>> OnEndAsync(CqrsContext context, CancellationToken ct = default);
}

public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand
{
    public virtual Task<Result<TResponse>> ValidateAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public virtual Task<Result<TResponse>> OnStartAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public abstract Task<Result<TResponse>> HandleAsync(CqrsContext context, CancellationToken ct = default);

    public virtual Task<Result<TResponse>> OnEndAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}