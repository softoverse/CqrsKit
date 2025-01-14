using Softoverse.CqrsKit.Abstraction.Handlers.Markers;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Handlers;

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandlerMarker
    where TCommand : ICommand
{
    Task<Response<TResponse>> ValidateAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Response<TResponse>> OnStartAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Response<TResponse>> HandleAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Response<TResponse>> OnEndAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}

public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand
{
    public virtual Task<Response<TResponse>> ValidateAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public virtual Task<Response<TResponse>> OnStartAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public abstract Task<Response<TResponse>> HandleAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    public virtual Task<Response<TResponse>> OnEndAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}