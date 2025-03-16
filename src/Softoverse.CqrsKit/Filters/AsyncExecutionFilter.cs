using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.Filters;

public class AsyncExecutionFilter<TRequest, TResponse>(List<IAsyncExecutionFilter> asyncExecutionFilters) :
    AsyncExecutionFilterBase<TRequest, TResponse>(asyncExecutionFilters)
    where TRequest : IRequest;