using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Abstraction.Services;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;

using Softoverse.CqrsKit.Builders;

namespace Softoverse.CqrsKit.Executors;

public sealed class CommandExecutor<TCommand, TResponse> : ICommandExecutor<TCommand, TResponse>,
                                                           ICommandApprovalFlowEventExecutor
    where TCommand : ICommand
{
    public required IServiceProvider Services { get; init; }

    public required IExecutionFilter<TCommand, TResponse> ExecutionFilter { get; init; }
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

        HandlerStep<TResponse>[] steps =
        [
            new(() => ExecuteApprovalFlowAsync(ct), isApprovalFlowRequired ? StepBehavior.FinalOutput : StepBehavior.Skip),
            new(() => ExecutionFilter.OnExecutingAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.MustCall),
            new(() => CommandHandler.ValidateAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => CommandHandler.OnStartAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => CommandHandler.HandleAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.FinalOutput),
            new(() => CommandHandler.OnEndAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => ExecutionFilter.OnExecutedAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.MustCall),
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Result<TResponse>> ExecuteApprovalFlowAsync(CancellationToken ct = default)
    {
        bool isApprovalFlowSkipped = ApprovalFlowHandler == null;
        HandlerStep<TResponse>[] steps =
        [
            new(() => CommandHandler.ValidateAsync(Context, ct)),
            new(() => IsApprovalFlowPendingTaskUniqueAsync(ct)),
            new(() => isApprovalFlowSkipped ? ResponseDefaults.DefaultResponse<TResponse>() : ApprovalFlowHandler!.OnStartAsync(Context, ct), isApprovalFlowSkipped ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => ApprovalFlowExecutionFilter.OnExecutingAsync(Context, ct)),
            new(() => ApprovalFlowExecutionFilter.ExecuteAsync(Context, ct), StepBehavior.FinalOutput),
            new(() => ApprovalFlowExecutionFilter.OnExecutedAsync(Context, ct)),
            new(() => isApprovalFlowSkipped ? ResponseDefaults.DefaultResponse<TResponse>() : ApprovalFlowHandler!.OnEndAsync(Context, ct), isApprovalFlowSkipped ? StepBehavior.Skip : StepBehavior.Mandatory)
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
        
        return !approvalFlowUniqueResponse.IsSuccessful ? approvalFlowUniqueResponse : Result<TResponse>.Success();
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
            ? await ApprovalFlowHandler.AfterAcceptAsync(Context, ct)
            : await ResponseDefaults.DefaultResponse();
    }

    public async Task<object> AfterRejectAsync(CancellationToken ct)
    {
        // Context.SetCurrentState("ApprovalFlowReject");

        return ApprovalFlowHandler is not null
            ? await ApprovalFlowHandler.AfterRejectAsync(Context, ct)
            : await ResponseDefaults.DefaultResponse();
    }

    private async Task<Result<TResponse>> ExecuteStepsAsync(HandlerStep<TResponse>[] steps)
    {
        Context.Request = Command;
        Context.SetApprovalFlowPendingTaskContextData(typeof(TCommand), typeof(ICommandHandler<TCommand, TResponse>), typeof(TResponse), typeof(IApprovalFlowHandler<TCommand, TResponse>));

        var response = await SequentialStepExecutor.ExecuteStepsAsync(steps, Context);
        Context.Response = response;
        return response ?? Result<TResponse>.Error()
                                              .WithMessage("An error occurred.");
    }
}