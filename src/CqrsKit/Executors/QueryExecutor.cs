using CqrsKit.Builders;

using CqrsKit.Abstraction.Executors;
using CqrsKit.Abstraction.Filters;
using CqrsKit.Abstraction.Handlers;

using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Executors;

public sealed class QueryExecutor<TQuery, TResponse> : IQueryExecutor<TQuery, TResponse>
    where TQuery : IQuery
{
    public required IServiceProvider Services { get; init; }
    public required IExecutionFilter<TQuery, TResponse> ExecutionFilter { get; init; }

    public required IQueryHandler<TQuery, TResponse> QueryHandler { get; init; }

    public required TQuery Query { get; init; }
    public CqrsContext Context { get; init; }

    public async Task<Response<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default)
    {
        return await QueryExecutorBuilder<TQuery, TResponse>.Initialize(Services)
                                                            .WithQuery(Query).WithDefaultHandler()
                                                            .WithItems(Context.Items)
                                                            .Build()
                                                            .ExecuteAsync(ct);
    }

    public async Task<Response<TResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        HandlerStep<TResponse>[] steps =
        [
            new(() => ExecutionFilter.OnExecutingAsync(Context, ct), StepBehavior.MustCall),
            new(() => QueryHandler.OnStartAsync(Query, Context, ct)),
            new(() => QueryHandler.HandleAsync(Query, Context, ct), StepBehavior.FinalOutput),
            new(() => QueryHandler.OnEndAsync(Query, Context, ct)),
            new(() => ExecutionFilter.OnExecutedAsync(Context, ct), StepBehavior.MustCall)
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Response<TResponse>> ExecuteStepsAsync(HandlerStep<TResponse>[] steps)
    {
        Context.Request = Query;
        var response = await SequentialStepExecutor.ExecuteStepsAsync(steps, Context);
        Context.Response = response;
        return response ?? Response<TResponse>.Error()
                                              .WithMessage("An error occurred.");
    }
}