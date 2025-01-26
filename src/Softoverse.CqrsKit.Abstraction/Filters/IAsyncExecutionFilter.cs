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