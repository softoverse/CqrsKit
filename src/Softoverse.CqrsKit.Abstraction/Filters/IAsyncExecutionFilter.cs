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

public abstract class AsyncExecutionFilterBase<TRequest, TResponse>(List<IAsyncExecutionFilter> asyncExecutionFilters) :
    IAsyncExecutionFilter<TRequest, TResponse>
    where TRequest : IRequest
{
    internal readonly List<IAsyncExecutionFilter> AsyncExecutionFilters = asyncExecutionFilters;

    public virtual async Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        foreach (var asyncExecutionFilter in AsyncExecutionFilters)
        {
            await asyncExecutionFilter.OnActionExecutingAsync(context, ct);
        }
    }

    public virtual async Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        AsyncExecutionFilters.Reverse();
        foreach (var asyncExecutionFilter in AsyncExecutionFilters)
        {
            await asyncExecutionFilter.OnActionExecutedAsync(context, ct);
        }
    }
}