using CqrsKit.Builders;

using CqrsKit.Abstraction.Executors;
using CqrsKit.Abstraction.Filters;
using CqrsKit.Abstraction.Handlers;
using CqrsKit.Abstraction.Services;

using CqrsKit.Extensions;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Executors;

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


    public async Task<Response<TResponse>> ExecuteDefaultAsync(CancellationToken ct = default)
    {
        return await CommandExecutorBuilder<TCommand, TResponse>.Initialize(Services)
                                                                .WithCommand(Command).WithDefaultHandler()
                                                                .WithItems(Context.Items)
                                                                .WithoutApprovalFlow()
                                                                .Build()
                                                                .ExecuteAsync(ct);
    }

    public async Task<Response<TResponse>> ExecuteAsync(CancellationToken ct = default)
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
            new(() => CommandHandler.ValidateAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => CommandHandler.OnStartAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => CommandHandler.HandleAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.FinalOutput),
            new(() => CommandHandler.OnEndAsync(Command, Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => ExecutionFilter.OnExecutedAsync(Context, ct), isApprovalFlowRequired ? StepBehavior.Skip : StepBehavior.MustCall),
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Response<TResponse>> ExecuteApprovalFlowAsync(CancellationToken ct = default)
    {
        bool isApprovalFlowSkipped = ApprovalFlowHandler == null;
        HandlerStep<TResponse>[] steps =
        [
            new(() => CommandHandler.ValidateAsync(Command, Context, ct)),
            new(() => IsApprovalFlowPendingTaskUniqueAsync(ct)),
            new(() => isApprovalFlowSkipped ? ResponseDefaults.DefaultResponse<TResponse>() : ApprovalFlowHandler!.OnStartAsync(Command, Context, ct), isApprovalFlowSkipped ? StepBehavior.Skip : StepBehavior.Mandatory),
            new(() => ApprovalFlowExecutionFilter.OnExecutingAsync(Command, Context, ct)),
            new(() => ApprovalFlowExecutionFilter.ExecuteAsync(Command, Context, ct), StepBehavior.FinalOutput),
            new(() => ApprovalFlowExecutionFilter.OnExecutedAsync(Command, Context, ct)),
            new(() => isApprovalFlowSkipped ? ResponseDefaults.DefaultResponse<TResponse>() : ApprovalFlowHandler!.OnEndAsync(Command, Context, ct), isApprovalFlowSkipped ? StepBehavior.Skip : StepBehavior.Mandatory)
        ];

        return await ExecuteStepsAsync(steps);
    }

    private async Task<Response<TResponse>> IsApprovalFlowPendingTaskUniqueAsync(CancellationToken ct = default)
    {
        if (Context.State == CurrentState.ApprovalFlowExecution)
            return Response<TResponse>.Success();
        
        bool isApprovalFlowPendingTaskUnique = await ApprovalFlowService.IsApprovalFlowPendingTaskUniqueAsync(Command, Context, ct);
        var approvalFlowUniqueResponse = Response<TResponse>.Create(isApprovalFlowPendingTaskUnique)
                                                            .WithErrorMessage("This command is already in approval flow");
        
        return !approvalFlowUniqueResponse.IsSuccessful ? approvalFlowUniqueResponse : Response<TResponse>.Success();
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
            : await ResponseDefaults.DefaultResponse();
    }

    public async Task<object> AfterRejectAsync(CancellationToken ct)
    {
        // Context.SetCurrentState("ApprovalFlowReject");

        return ApprovalFlowHandler is not null
            ? await ApprovalFlowHandler.AfterRejectAsync(Command, Context, ct)
            : await ResponseDefaults.DefaultResponse();
    }

    private async Task<Response<TResponse>> ExecuteStepsAsync(HandlerStep<TResponse>[] steps)
    {
        Context.Request = Command;
        Context.SetApprovalFlowPendingTaskContextData(typeof(TCommand), typeof(ICommandHandler<TCommand, TResponse>), typeof(TResponse), typeof(IApprovalFlowHandler<TCommand, TResponse>));

        var response = await SequentialStepExecutor.ExecuteStepsAsync(steps, Context);
        Context.Response = response;
        return response ?? Response<TResponse>.Error()
                                              .WithMessage("An error occurred.");
    }
}