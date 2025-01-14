using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Abstraction.Filters;

public interface IExecutionFilterMarker;

public interface IExecutionFilter<in TRequest, TResponse> : IExecutionFilterMarker
    where TRequest : IRequest
{
    public Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default);

    public Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default);
}