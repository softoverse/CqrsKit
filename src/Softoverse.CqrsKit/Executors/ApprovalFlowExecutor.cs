using System.Text;
using System.Text.Json;

using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Abstraction.Services;

using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Entity;
using Softoverse.CqrsKit.Model.Errors;
using Softoverse.CqrsKit.Model.Utility;

using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Executors;

public sealed class ApprovalFlowExecutor<T> : IApprovalFlowExecutor<T>
    where T : BaseApprovalFlowPendingTask
{
    public required IServiceProvider Services { get; init; }

    public required IApprovalFlowService ApprovalFlowService { get; init; }
    public required IApprovalFlowAcceptFilter ApprovalFlowAcceptFilter { get; init; }
    public required IApprovalFlowRejectFilter ApprovalFlowRejectFilter { get; init; }

    public required string ApprovalFlowPendingTaskId { get; init; }

    public CqrsContext Context { get; init; }

    public async Task<Result> AcceptAsync(CancellationToken ct = default)
    {
        Context.ApprovalFlowPendingTaskId = ApprovalFlowPendingTaskId;
        
        Result beforeApprovalFlowAcceptResult = await ApprovalFlowAcceptFilter.OnExecutingAsync(Context, ct);
        if (!beforeApprovalFlowAcceptResult.IsSuccessful) return beforeApprovalFlowAcceptResult;

        var commandExecutorResponse = await GetCommandApprovalFlowExecutor(ct);
        if (!commandExecutorResponse.IsSuccessful) return commandExecutorResponse;
        ICommandApprovalFlowEventExecutor commandExecutor = commandExecutorResponse.Payload;

        Result commandResult = await commandExecutor.ExecuteDynamicAsync(ct) as Result ?? Result.Error();

        var afterAcceptResponse = await commandExecutor.AfterAcceptAsync(ct) as Result ?? Result.Success();
        if (!afterAcceptResponse.IsSuccessful) return afterAcceptResponse;

        Result afterApprovalFlowAcceptResult = await ApprovalFlowAcceptFilter.OnExecutedAsync(Context, ct);
        if (!afterApprovalFlowAcceptResult.IsSuccessful) return afterApprovalFlowAcceptResult;

        return commandResult;
    }

    public async Task<Result> RejectAsync(CancellationToken ct = default)
    {
        Context.ApprovalFlowPendingTaskId = ApprovalFlowPendingTaskId;
        
        var onRejectResponse = await ApprovalFlowRejectFilter.OnExecutingAsync(Context, ct);
        if (!onRejectResponse.IsSuccessful) return onRejectResponse;

        var commandExecutorResponse = await GetCommandApprovalFlowExecutor(ct);
        if (!commandExecutorResponse.IsSuccessful) return commandExecutorResponse;
        ICommandApprovalFlowEventExecutor commandExecutor = commandExecutorResponse.Payload;

        Result afterRejectResult = await commandExecutor.AfterRejectAsync(ct) as Result ?? Result.Success();
        if (!afterRejectResult.IsSuccessful) return afterRejectResult;
        
        Result afterApprovalFlowRejectResult = await ApprovalFlowRejectFilter.OnExecutedAsync(Context, ct);
        if (!afterApprovalFlowRejectResult.IsSuccessful) return afterApprovalFlowRejectResult;

        return afterRejectResult;
    }

    private async Task<Result<ICommandApprovalFlowEventExecutor>> GetCommandApprovalFlowExecutor(CancellationToken ct = default)
    {
        Context.ApprovalFlowPendingTaskId = ApprovalFlowPendingTaskId;
        
        T pendingTask = await ApprovalFlowService.GetApprovalFlowTaskAsync<T>(Context, ct);
        if (pendingTask == null!)
        {
            return Result<ICommandApprovalFlowEventExecutor>.Error(ApprovalFlowPendingTaskErrors.NotFound(ApprovalFlowPendingTaskId));
        }

        Type? commandType = CqrsHelper.GetType(pendingTask.CommandFullName);
        if (commandType == null)
        {
            return Result<ICommandApprovalFlowEventExecutor>.Error(CommandErrors.NotFound(pendingTask.CommandName));
        }

        Result<object> payloadParseResult = ParsePayload(commandType, pendingTask);
        if (!payloadParseResult.IsSuccessful)
            return Result<ICommandApprovalFlowEventExecutor>.Error()
                                                              .WithMessage(payloadParseResult.Message!)
                                                              .WithErrors(payloadParseResult.Errors!);

        object commandObjectInType = payloadParseResult.Payload;

        var commandExecutor = CommandExecutorDynamicBuilder.Initialize(Services)
                                                           .BuildCommandExecutor(Context,
                                                                                 pendingTask.CommandFullName,
                                                                                 pendingTask.ResponseFullName!,
                                                                                 pendingTask.HandlerFullName!,
                                                                                 pendingTask.ApprovalFlowHandlerFullName!,
                                                                                 commandObjectInType);

        return Result<ICommandApprovalFlowEventExecutor>.Success()
                                                          .WithPayload(commandExecutor);
    }

    private static Result<object> ParsePayload(Type commandType, T pendingTask)
    {
        JsonSerializerOptions serializerOptions = CqrsHelper.GetDefaultSerializerOption();
        var payloadInString = Encoding.ASCII.GetString(pendingTask.Payload ?? []);
        object? commandObjectInType = JsonSerializer.Deserialize(payloadInString, commandType, serializerOptions);

        if (commandObjectInType != null)
        {
            return Result<object>.Success()
                                   .WithPayload(commandObjectInType);
        }

        return Result<object>.Error(ApprovalFlowPendingTaskErrors.InvalidPayload(commandType.Name));
    }
}