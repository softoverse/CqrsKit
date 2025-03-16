using Softoverse.CqrsKit.Abstractions.Executors;
using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Abstractions.Services;
using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Executors;

public sealed class CommandExecutor<TCommand, TResponse> : ICommandExecutor<TCommand, TResponse>,
                                                           ICommandApprovalFlowEventExecutor
    where TCommand : ICommand
{
    public required IServiceProvider Services { get; init; }

    public required IExecutionFilter<TCommand, TResponse> ExecutionFilter { get; init; }
    public required IAsyncExecutionFilter<TCommand, TResponse>? AsyncExecutionFilter { get; init; }
    public required IApprovalFlowExecutionFilter<TCommand, TResponse> ApprovalFlowExecutionFilter { get; init; }

    public required IApprovalFlowService ApprovalFlowService { get; init; }

    public required ICommandHandler<TCommand, TResponse> CommandHandler { get; init; }
    public IApprovalFlowHandler<TCommand, TResponse>? ApprovalFlowHandler { get; init; }

    public required TCommand Command { get; init; }
    public CqrsContext Context { get; init; }
    public required bool IsApprovalFlowEnabled { get; init; }


    public async Task<Result<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default)
    {
        return await CommandExecutorBuilder<TCommand, TResponse>.Initialize(Services)
                                                                .WithCommand(Command).WithDefaultHandler()
                                                                .WithItems(Context.Items)
                                                                .WithoutApprovalFlow()
                                                                .Build()
                                                                .ExecuteAsync(ct);
    }

    public async Task<Result<TResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        bool isApprovalFlowRequired = false;
        if (IsApprovalFlowEnabled)
        {
            isApprovalFlowRequired = await ApprovalFlowService.IsApprovalFlowRequiredAsync(Context, typeof(TCommand), ct: ct);
        }
        
        var isAsyncExecutionFilterAvailable = AsyncExecutionFilter != null;

        HandlerStep<TResponse>[] steps =
        [
            HandlerStep<TResponse>.New(() => ExecutionFilter.OnExecutingAsync(Command, Context, ct), StepBehavior.MustCall),
            HandlerStep<TResponse>.New(() => ExecuteAsyncExecutionFilterActionAsync(() => AsyncExecutionFilter?.OnExecutingAsync(Context, ct)!), isAsyncExecutionFilterAvailable ? StepBehavior.MustCall : StepBehavior.Skip),
            HandlerStep<TResponse>.New(() => ExecuteApprovalFlowAsync(ct), isApprovalFlowRequired ? StepBehavior.FinalOutput : StepBehavior.Skip),
            HandlerStep<TResponse>.New(() => CommandHandler.ValidateAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            HandlerStep<TResponse>.New(() => CommandHandler.OnStartAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            HandlerStep<TResponse>.New(() => CommandHandler.HandleAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.FinalOutput),
            HandlerStep<TResponse>.New(() => CommandHandler.OnEndAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            HandlerStep<TResponse>.New(() => ExecuteAsyncExecutionFilterActionAsync(() => AsyncExecutionFilter?.OnExecutedAsync(Context, ct)!), isAsyncExecutionFilterAvailable ? StepBehavior.MustCall : StepBehavior.Skip),
            HandlerStep<TResponse>.New(() => ExecutionFilter.OnExecutedAsync(Command, Context, ct), StepBehavior.MustCall)
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Result<TResponse>> ExecuteApprovalFlowAsync(CancellationToken ct = default)
    {
        bool isApprovalFlowSkipped = ApprovalFlowHandler == null;
        HandlerStep<TResponse>[] steps =
        [
            HandlerStep<TResponse>.New(() => CommandHandler.ValidateAsync(Command, Context, ct)),
            HandlerStep<TResponse>.New(() => IsApprovalFlowPendingTaskUniqueAsync(ct)),
            HandlerStep<TResponse>.New(() => isApprovalFlowSkipped ? ResultDefaults.DefaultResult<TResponse>() : ApprovalFlowHandler!.OnStartAsync(Command, Context, ct), isApprovalFlowSkipped ? StepBehavior.Skip : StepBehavior.Mandatory),
            HandlerStep<TResponse>.New(() => ApprovalFlowExecutionFilter.OnExecutingAsync(Command, Context, ct)),
            HandlerStep<TResponse>.New(() => ApprovalFlowExecutionFilter.ExecuteAsync(Command, Context, ct), StepBehavior.FinalOutput),
            HandlerStep<TResponse>.New(() => ApprovalFlowExecutionFilter.OnExecutedAsync(Command, Context, ct)),
            HandlerStep<TResponse>.New(() => isApprovalFlowSkipped ? ResultDefaults.DefaultResult<TResponse>() : ApprovalFlowHandler!.OnEndAsync(Command, Context, ct), isApprovalFlowSkipped ? StepBehavior.Skip : StepBehavior.Mandatory)
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Result<TResponse>> IsApprovalFlowPendingTaskUniqueAsync(CancellationToken ct = default)
    {
        if (Context.State == CurrentState.ApprovalFlowExecution)
            return Result<TResponse>.Success();
        
        bool isApprovalFlowPendingTaskUnique = await ApprovalFlowService.IsApprovalFlowPendingTaskUniqueAsync(Command, Context, ct);
        var approvalFlowUniqueResponse = Result<TResponse>.Create(isApprovalFlowPendingTaskUnique)
                                                            .WithErrorMessage("This command is already in approval flow");
        
        return approvalFlowUniqueResponse.IsFailure ? approvalFlowUniqueResponse : Result<TResponse>.Success();
    }

    public async Task<object> ExecuteDynamicAsync(CancellationToken ct)
    {
        Context.State = CurrentState.ApprovalFlowExecution;
        return await ExecuteAsync(ct);
    }

    public async Task<object> AfterAcceptAsync(CancellationToken ct)
    {
        // Context.SetCurrentState("ApprovalFlowAccept");

        return ApprovalFlowHandler is not null
            ? await ApprovalFlowHandler.AfterAcceptAsync(Command, Context, ct)
            : await ResultDefaults.DefaultResult();
    }

    public async Task<object> AfterRejectAsync(CancellationToken ct)
    {
        // Context.SetCurrentState("ApprovalFlowReject");

        return ApprovalFlowHandler is not null
            ? await ApprovalFlowHandler.AfterRejectAsync(Command, Context, ct)
            : await ResultDefaults.DefaultResult();
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
        Context.Request = Command;
        Context.SetApprovalFlowPendingTaskContextData(typeof(TCommand), typeof(ICommandHandler<TCommand, TResponse>), typeof(TResponse), typeof(IApprovalFlowHandler<TCommand, TResponse>));

        var response = await SequentialStepExecutor.ExecuteStepsAsync(steps, Context);
        Context.Result = response;
        return response ?? Result<TResponse>.Error()
                                              .WithMessage("An error occurred.");
    }
}