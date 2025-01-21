using System.Text;
using System.Text.Json;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Entity;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Services;

public class ApprovalFlowService : ApprovalFlowServiceBase
{
    public override Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.IsApprovalFlowRequiredAsync)}");

        List<string> requiredCommands = [nameof (PersonDeleteCommand)];

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

    public override Task<T> GetApprovalFlowTaskAsync<T>(CqrsContext context, CancellationToken ct = default) where T : class
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.GetApprovalFlowTaskAsync)}");

        // Here it is hard coded.
        // In real life implementation it will be generated dynamically from DB or other source
        Type studentDeleteCommandType = typeof(PersonDeleteCommand);
        Type studentDeleteCommandHandlerType = typeof(ICommandHandler<PersonDeleteCommand, Guid>);
        Type studentDeleteApprovalFlowHandlerType = typeof(IApprovalFlowHandler<PersonDeleteCommand, Guid>);

        var approvalFlowPendingTaskId = Guid.Parse(context.ApprovalFlowPendingTaskId);

        // Here it is hard coded.
        // In real life implementation it will be generated dynamically from DB or other source
        IUniqueCommand uniqueCommand = new PersonDeleteCommand(approvalFlowPendingTaskId);

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
            Payload = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(new PersonDeleteCommand(approvalFlowPendingTaskId))),
            Status = ApprovalFlowStatus.Pending,
            UniqueIdentification = uniqueCommand.GetUniqueIdentification(),
            Id = context.ApprovalFlowPendingTaskId
        };

        return Task.FromResult(approvalFlowTask as T)!;
    }
}