using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

public abstract class CommandExecutionFilterBase<TCommand, TResponse> : IExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
    public abstract Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

internal class CommandExecutionFilter<TCommand, TResponse> : CommandExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}