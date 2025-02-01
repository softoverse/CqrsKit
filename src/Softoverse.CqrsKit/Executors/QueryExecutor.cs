using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Filters.Attributes;

namespace Softoverse.CqrsKit.Executors;

public sealed class QueryExecutor<TQuery, TResponse> : IQueryExecutor<TQuery, TResponse>
    where TQuery : IQuery
{
    public required IServiceProvider Services { get; init; }
    public required IExecutionFilter<TQuery, TResponse> ExecutionFilter { get; init; }
    public required IAsyncExecutionFilter<TQuery, TResponse>? AsyncExecutionFilter { get; init; }

    public required IQueryHandler<TQuery, TResponse> QueryHandler { get; init; }

    public required TQuery Query { get; init; }
    public CqrsContext Context { get; init; }

    public async Task<Result<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default)
    {
        return await QueryExecutorBuilder<TQuery, TResponse>.Initialize(Services)
                                                            .WithQuery(Query).WithDefaultHandler()
                                                            .WithItems(Context.Items)
                                                            .Build()
                                                            .ExecuteAsync(ct);
    }

    public async Task<Result<TResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var isAsyncExecutionFilterAvailable = AsyncExecutionFilter != null;

        HandlerStep<TResponse>[] steps =
        [
            HandlerStep<TResponse>.New(() => ExecutionFilter.OnExecutingAsync(Query, Context, ct), StepBehavior.MustCall),
            HandlerStep<TResponse>.New(() => ExecuteAsyncExecutionFilterActionAsync(() => AsyncExecutionFilter?.OnExecutingAsync(Context, ct)!), isAsyncExecutionFilterAvailable ? StepBehavior.MustCall : StepBehavior.Skip),
            HandlerStep<TResponse>.New(() => QueryHandler.OnStartAsync(Query, Context, ct)),
            HandlerStep<TResponse>.New(() => QueryHandler.HandleAsync(Query, Context, ct), StepBehavior.FinalOutput),
            HandlerStep<TResponse>.New(() => QueryHandler.OnEndAsync(Query, Context, ct)),
            HandlerStep<TResponse>.New(() => ExecuteAsyncExecutionFilterActionAsync(() => AsyncExecutionFilter?.OnExecutedAsync(Context, ct)!), isAsyncExecutionFilterAvailable ? StepBehavior.MustCall : StepBehavior.Skip),
            HandlerStep<TResponse>.New(() => ExecutionFilter.OnExecutedAsync(Query, Context, ct), StepBehavior.MustCall)
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Result<TResponse>> ExecuteAsyncExecutionFilterActionAsync(Func<Task>? action)
    {
        if (action == null)
        {
            return await ResultDefaults.DefaultResult<TResponse>();
        }

        await action();
        return Context.ResultAs<TResponse>();
    }

    private async Task<Result<TResponse>> ExecuteStepsAsync(HandlerStep<TResponse>[] steps)
    {
        Context.Request = Query;
        Context.SetApprovalFlowPendingTaskContextData(typeof(TQuery), typeof(IQueryHandler<TQuery, TResponse>), typeof(TResponse), null);

        var response = await SequentialStepExecutor.ExecuteStepsAsync(steps, Context);
        Context.Result = response;
        return response ?? Result<TResponse>.Error()
                                            .WithMessage("An error occurred.");
    }
}