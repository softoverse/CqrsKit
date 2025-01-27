using System.Text;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Entity;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.WebApi.CQRS.Services;

public class ApprovalFlowService : ApprovalFlowServiceBase
{
    // public override Task<bool> IsApprovalFlowRequiredAsync(CqrsContext context, Type commandType, Type? responseType = null, CancellationToken ct = default)
    // {
    //     // List<string> requiredCommands = [nameof (StudentDeleteCommand)];
    //     //
    //     // var isApprovalFlowRequired = Program.IsApprovalFlowEnabled && requiredCommands.Contains(commandType.Name);
    //     //
    //     // return Task.FromResult(isApprovalFlowRequired);
    //     return Task.FromResult(isApprovalFlowRequired);
    // }
    //
    // public override Task<bool> IsApprovalFlowPendingTaskUniqueAsync<TCommand>(TCommand command, CqrsContext context, CancellationToken ct = default)
    // {
    //     var uniqueCommand = command as IUniqueCommand;
    //
    //     string? uniqueIdentification = uniqueCommand?.GetUniqueIdentification();
    //     // Check if the unique identification is unique in the system
    //
    //     return Task.FromResult(true);
    // }
    //
    // public override Task<T?> GetApprovalFlowTaskAsync<T>(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    // {
    //
    //     Type studentDeleteCommandType = typeof(StudentDeleteCommand);
    //     Type studentDeleteCommandHandlerType = typeof(ICommandHandler<StudentDeleteCommand, Guid>);
    //     Type studentDeleteApprovalFlowHandlerType = typeof(IApprovalFlowHandler<StudentDeleteCommand, Guid>);
    //
    //     var afId = Guid.Parse(approvalFlowId);
    //
    //     BaseApprovalFlowPendingTask approvalFlowTask = new BaseApprovalFlowPendingTask
    //     {
    //         CommandName = studentDeleteCommandType.Name,
    //         CommandNamespace = studentDeleteCommandType.Namespace!,
    //         CommandFullName = studentDeleteCommandType.FullName!,
    //         ResponseName = nameof (Guid),
    //         ResponseNamespace = typeof(Guid).Namespace,
    //         ResponseFullName = typeof(Guid).FullName,
    //         HandlerName = studentDeleteCommandHandlerType.Name,
    //         HandlerNamespace = studentDeleteCommandHandlerType.Namespace,
    //         HandlerFullName = studentDeleteCommandHandlerType.FullName,
    //         ApprovalFlowHandlerName = studentDeleteApprovalFlowHandlerType.Name,
    //         ApprovalFlowHandlerNamespace = studentDeleteApprovalFlowHandlerType.Namespace,
    //         ApprovalFlowHandlerFullName = studentDeleteApprovalFlowHandlerType.FullName,
    //         Payload = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(new StudentDeleteCommand(afId))),
    //         Status = ApprovalFlowStatus.Pending,
    //         UniqueIdentification = approvalFlowId,
    //         Id = approvalFlowId
    //     };
    //
    //     return Task.FromResult(approvalFlowTask as T)!;
    // }
}