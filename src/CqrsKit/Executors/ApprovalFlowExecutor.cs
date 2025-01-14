using System.Text;
using System.Text.Json;

using CqrsKit.Builders;
using CqrsKit.Services;

using CqrsKit.Abstraction.Executors;
using CqrsKit.Abstraction.Filters;
using CqrsKit.Abstraction.Services;

using CqrsKit.Model;
using CqrsKit.Model.Entity;
using CqrsKit.Model.Errors;
using CqrsKit.Model.Utility;

namespace CqrsKit.Executors;

public sealed class ApprovalFlowExecutor<T> : IApprovalFlowExecutor<T>
    where T : BaseApprovalFlowPendingTask
{
    public required IServiceProvider Services { get; init; }

    public required IApprovalFlowService ApprovalFlowService { get; init; }
    public required IApprovalFlowAcceptFilter ApprovalFlowAcceptFilter { get; init; }
    public required IApprovalFlowRejectFilter ApprovalFlowRejectFilter { get; init; }

    public required string ApprovalFlowPendingTaskId { get; init; }

    public CqrsContext Context { get; init; }

    public async Task<Response> AcceptAsync(CancellationToken ct = default)
    {
        Response beforeApprovalFlowAcceptResponse = await ApprovalFlowAcceptFilter.OnAcceptingAsync(ApprovalFlowPendingTaskId, Context, ct);
        if (!beforeApprovalFlowAcceptResponse.IsSuccessful) return beforeApprovalFlowAcceptResponse;

        var commandExecutorResponse = await GetCommandApprovalFlowExecutor(ct);
        if (!commandExecutorResponse.IsSuccessful) return commandExecutorResponse;
        ICommandApprovalFlowEventExecutor commandExecutor = commandExecutorResponse.Payload;

        Response commandResponse = await commandExecutor.ExecuteDynamicAsync(ct) as Response ?? Response.Error();

        var afterAcceptResponse = await commandExecutor.AfterAcceptAsync(ct) as Response ?? Response.Success();
        if (!afterAcceptResponse.IsSuccessful) return afterAcceptResponse;

        Response afterApprovalFlowAcceptResponse = await ApprovalFlowAcceptFilter.OnAcceptedAsync(ApprovalFlowPendingTaskId, Context, ct);
        if (!afterApprovalFlowAcceptResponse.IsSuccessful) return afterApprovalFlowAcceptResponse;

        return commandResponse;
    }

    public async Task<Response> RejectAsync(CancellationToken ct = default)
    {
        var onRejectResponse = await ApprovalFlowRejectFilter.OnRejectingAsync(ApprovalFlowPendingTaskId, Context, ct);
        if (!onRejectResponse.IsSuccessful) return onRejectResponse;

        var commandExecutorResponse = await GetCommandApprovalFlowExecutor(ct);
        if (!commandExecutorResponse.IsSuccessful) return commandExecutorResponse;
        ICommandApprovalFlowEventExecutor commandExecutor = commandExecutorResponse.Payload;

        Response afterRejectResponse = await commandExecutor.AfterRejectAsync(ct) as Response ?? Response.Success();
        if (!afterRejectResponse.IsSuccessful) return afterRejectResponse;

        Response afterApprovalFlowRejectResponse = await ApprovalFlowRejectFilter.OnRejectedAsync(ApprovalFlowPendingTaskId, Context, ct);
        if (!afterApprovalFlowRejectResponse.IsSuccessful) return afterApprovalFlowRejectResponse;

        return afterRejectResponse;
    }

    private async Task<Response<ICommandApprovalFlowEventExecutor>> GetCommandApprovalFlowExecutor(CancellationToken ct = default)
    {
        T? pendingTask = await ApprovalFlowService.GetApprovalFlowTaskAsync<T>(ApprovalFlowPendingTaskId, Context, ct);
        if (pendingTask is null)
        {
            return Response<ICommandApprovalFlowEventExecutor>.Error(ApprovalFlowPendingTaskErrors.NotFound(ApprovalFlowPendingTaskId));
        }

        Type? commandType = CqrsHelper.GetType(pendingTask.CommandFullName);
        if (commandType == null)
        {
            return Response<ICommandApprovalFlowEventExecutor>.Error(CommandErrors.NotFound(pendingTask.CommandName));
        }

        Response<object> payloadParseResponse = ParsePayload(commandType, pendingTask);
        if (!payloadParseResponse.IsSuccessful)
            return Response<ICommandApprovalFlowEventExecutor>.Error()
                                                              .WithMessage(payloadParseResponse.Message!)
                                                              .WithErrors(payloadParseResponse.Errors!);

        object commandObjectInType = payloadParseResponse.Payload;

        var commandExecutor = CommandExecutorDynamicBuilder.Initialize(Services)
                                                           .BuildCommandExecutor(Context,
                                                                                 pendingTask.CommandFullName,
                                                                                 pendingTask.ResponseFullName!,
                                                                                 pendingTask.HandlerFullName!,
                                                                                 pendingTask.ApprovalFlowHandlerFullName!,
                                                                                 commandObjectInType);

        return Response<ICommandApprovalFlowEventExecutor>.Success()
                                                          .WithPayload(commandExecutor);
    }

    private static Response<object> ParsePayload(Type commandType, T pendingTask)
    {
        JsonSerializerOptions serializerOptions = CqrsHelper.GetDefaultSerializerOption();
        var payloadInString = Encoding.ASCII.GetString(pendingTask.Payload ?? []);
        object? commandObjectInType = JsonSerializer.Deserialize(payloadInString, commandType, serializerOptions);

        if (commandObjectInType != null)
        {
            return Response<object>.Success()
                                   .WithPayload(commandObjectInType);
        }

        return Response<object>.Error(ApprovalFlowPendingTaskErrors.InvalidPayload(commandType.Name));
    }
}