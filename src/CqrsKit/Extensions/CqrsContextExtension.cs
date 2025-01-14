using CqrsKit.Model.Utility;

namespace CqrsKit.Extensions;

internal static class CqrsContextExtension
{
    public static void SetApprovalFlowPendingTaskContextData(this CqrsContext context, Type commandType, Type commandHandlerType, Type? responseType, Type? approvalFlowHandlerType)
    {
        context.CommandName = commandType.Name;
        context.CommandNamespace = commandType.Namespace;
        context.CommandFullName = commandType.FullName;

        context.HandlerName = commandHandlerType.Name;
        context.HandlerNamespace = commandHandlerType.Namespace;
        context.HandlerFullName = commandHandlerType.FullName;

        context.ResponseName = responseType?.Name;
        context.ResponseNamespace = responseType?.Namespace;
        context.ResponseFullName = responseType?.FullName;

        context.ApprovalFlowHandlerName = approvalFlowHandlerType?.Name;
        context.ApprovalFlowHandlerNamespace = approvalFlowHandlerType?.Namespace;
        context.ApprovalFlowHandlerFullName = approvalFlowHandlerType?.FullName;
    }
}