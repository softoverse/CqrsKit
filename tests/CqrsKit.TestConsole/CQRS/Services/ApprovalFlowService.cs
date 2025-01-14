﻿using System.Text;
using System.Text.Json;

using CqrsKit.Abstraction.Handlers;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Entity;
using CqrsKit.Model.Utility;
using CqrsKit.Services;
using CqrsKit.TestConsole.CQRS.Events;
using CqrsKit.TestConsole.CQRS.Events.Command;

namespace CqrsKit.TestConsole.CQRS.Services;

public class ApprovalFlowService : ApprovalFlowServiceBase
{
    public override Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.IsApprovalFlowRequiredAsync)}");

        List<string> requiredCommands = [nameof (StudentDeleteCommand)];

        var isApprovalFlowRequired = Program.IsApprovalFlowEnabled && requiredCommands.Contains(commandType.Name);

        return Task.FromResult(isApprovalFlowRequired);
    }

    public override Task<bool> IsApprovalFlowPendingTaskUniqueAsync<TCommand>(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.IsApprovalFlowPendingTaskUniqueAsync)}<{typeof(TCommand).Name}>");

        var uniqueCommand = command as IUniqueCommand;

        string? uniqueIdentification = uniqueCommand?.GetUniqueIdentification();
        // Check if the unique identification is unique in the system

        return Task.FromResult(true);
    }

    public override Task<T> GetApprovalFlowTaskAsync<T>(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetApprovalFlowTaskAsync)}");

        Type studentDeleteCommandType = typeof(StudentDeleteCommand);
        Type studentDeleteCommandHandlerType = typeof(ICommandHandler<StudentDeleteCommand, Guid>);
        Type studentDeleteApprovalFlowHandlerType = typeof(IApprovalFlowHandler<StudentDeleteCommand, Guid>);

        var afId = Guid.Parse(approvalFlowId);

        BaseApprovalFlowPendingTask approvalFlowTask = new BaseApprovalFlowPendingTask
        {
            CommandName = studentDeleteCommandType.Name,
            CommandNamespace = studentDeleteCommandType.Namespace!,
            CommandFullName = studentDeleteCommandType.FullName!,
            ResponseName = nameof (Guid),
            ResponseNamespace = typeof(Guid).Namespace,
            ResponseFullName = typeof(Guid).FullName,
            HandlerName = studentDeleteCommandHandlerType.Name,
            HandlerNamespace = studentDeleteCommandHandlerType.Namespace,
            HandlerFullName = studentDeleteCommandHandlerType.FullName,
            ApprovalFlowHandlerName = studentDeleteApprovalFlowHandlerType.Name,
            ApprovalFlowHandlerNamespace = studentDeleteApprovalFlowHandlerType.Namespace,
            ApprovalFlowHandlerFullName = studentDeleteApprovalFlowHandlerType.FullName,
            Payload = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(new StudentDeleteCommand(afId))),
            Status = ApprovalFlowStatus.Pending,
            UniqueIdentification = approvalFlowId,
            Id = approvalFlowId
        };

        return Task.FromResult(approvalFlowTask as T)!;
    }
}