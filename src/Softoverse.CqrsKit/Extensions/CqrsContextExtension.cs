using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Extensions;

internal static class CqrsContextExtension
{
    public static void SetApprovalFlowPendingTaskContextData(this CqrsContext context, Type requestType, Type requestHandlerType, Type? responseType, Type? approvalFlowHandlerType)
    {
        context.RequestName = requestType.Name;
        context.RequestNamespace = requestType.Namespace!;
        context.RequestFullName = requestType.FullName!;

        context.HandlerName = requestHandlerType.Name;
        context.HandlerNamespace = requestHandlerType.Namespace!;
        context.HandlerFullName = requestHandlerType.FullName!;

        context.ResponseName = responseType?.Name!;
        context.ResponseNamespace = responseType?.Namespace!;
        context.ResponseFullName = responseType?.FullName!;

        context.ApprovalFlowHandlerName = approvalFlowHandlerType?.Name!;
        context.ApprovalFlowHandlerNamespace = approvalFlowHandlerType?.Namespace!;
        context.ApprovalFlowHandlerFullName = approvalFlowHandlerType?.FullName!;
    }
}