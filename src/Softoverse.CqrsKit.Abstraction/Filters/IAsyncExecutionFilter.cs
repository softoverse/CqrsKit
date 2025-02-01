using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Abstraction.Filters;

public interface IAsyncExecutionFilter
{
    public Task OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public Task OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
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

    public virtual async Task OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        foreach (var asyncExecutionFilter in AsyncExecutionFilters)
        {
            await asyncExecutionFilter.OnExecutingAsync(context, ct);
        }
    }

    public virtual async Task OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        AsyncExecutionFilters.Reverse();
        foreach (var asyncExecutionFilter in AsyncExecutionFilters)
        {
            await asyncExecutionFilter.OnExecutedAsync(context, ct);
        }
    }
}