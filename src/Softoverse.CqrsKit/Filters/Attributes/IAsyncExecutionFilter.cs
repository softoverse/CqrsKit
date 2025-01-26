using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters.Attributes;

public interface IAsyncExecutionFilter
{
    public Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default);
}

public interface IAsyncExecutionFilter<TRequest, TResponse> : IAsyncExecutionFilter
    where TRequest : IRequest
{
}

public class AsyncExecutionFilter<TRequest, TResponse>(IAsyncExecutionFilter asyncExecutionFilter) : IAsyncExecutionFilter<TRequest, TResponse>
    where TRequest : IRequest
{

    public Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return asyncExecutionFilter.OnActionExecutingAsync(context, ct);
    }

    public Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return asyncExecutionFilter.OnActionExecutedAsync(context, ct);
    }
}