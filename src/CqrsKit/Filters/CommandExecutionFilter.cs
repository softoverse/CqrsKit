using CqrsKit.Abstraction.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Filters;

public abstract class CommandExecutionFilterBase<TCommand, TResponse> : IExecutionFilter<TCommand, TResponse>
    where TCommand : ICommand
{
    public abstract Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public abstract Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public class CommandExecutionFilter<TCommand, TResponse> : CommandExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();

    public override Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default) => ResponseDefaults.DefaultResponse<TResponse>();
}