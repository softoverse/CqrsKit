using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters
{
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
}