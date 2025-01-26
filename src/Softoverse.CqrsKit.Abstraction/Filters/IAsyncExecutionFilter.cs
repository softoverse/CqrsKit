using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IAsyncExecutionFilter
{
    public Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public interface IAsyncExecutionFilter<TRequest, TResponse> : IAsyncExecutionFilter
    where TRequest : IRequest
{
}

public abstract class AsyncExecutionFilterBase<TRequest, TResponse>(IAsyncExecutionFilter asyncExecutionFilter) :
    IAsyncExecutionFilter<TRequest, TResponse>
    where TRequest : IRequest
{
    internal readonly IAsyncExecutionFilter AsyncExecutionFilter = asyncExecutionFilter;

    public virtual Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return AsyncExecutionFilter.OnActionExecutingAsync(context, ct);
    }

    public virtual Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return AsyncExecutionFilter.OnActionExecutedAsync(context, ct);
    }
}