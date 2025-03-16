using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Models.Errors;

public static class ApprovalFlowPendingTaskErrors
{
    public static CqrsError NotFound(string approvalFlowPendingTaskId) => CqrsError.Create("ApprovalFlowPendingTask.NotFound",
                                                                                           $"No Approval Flow Pending Task item with Id = '{approvalFlowPendingTaskId}' was found.");
    public static CqrsError InvalidPayload(string commandName) => CqrsError.Create("ApprovalFlowPendingTask.InvalidPayload",
                                                                                   string.IsNullOrEmpty(commandName) ? $"Could not parse the payload of type '{commandName}'." : "Could not parse the payload");
}