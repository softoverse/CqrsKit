using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters;

public class AsyncExecutionFilter<TRequest, TResponse>(IAsyncExecutionFilter asyncExecutionFilter) :
    AsyncExecutionFilterBase<TRequest, TResponse>(asyncExecutionFilter)
    where TRequest : IRequest;