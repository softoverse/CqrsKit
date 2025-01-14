using Softoverse.CqrsKit.Abstraction.Handlers.Markers;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Handlers;

public interface IApprovalFlowHandler<in TCommand, TResponse> : IApprovalFlowHandlerMarker
    where TCommand : ICommand
{
    Task<Response<TResponse>> OnStartAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Response<TResponse>> OnEndAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Response<TResponse>> AfterAcceptAsync(TCommand command, CqrsContext context, CancellationToken ct = default);

    Task<Response<TResponse>> AfterRejectAsync(TCommand command, CqrsContext context, CancellationToken ct = default);
}

public abstract class ApprovalFlowHandler<TCommand, TResponse> : IApprovalFlowHandler<TCommand, TResponse>
    where TCommand : ICommand
{
    public  virtual Task<Response<TResponse>> OnStartAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public  virtual Task<Response<TResponse>> OnEndAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public  virtual Task<Response<TResponse>> AfterAcceptAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public  virtual Task<Response<TResponse>> AfterRejectAsync(TCommand command, CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}