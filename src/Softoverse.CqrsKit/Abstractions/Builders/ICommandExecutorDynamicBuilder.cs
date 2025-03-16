using Softoverse.CqrsKit.Abstractions.Executors;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Abstractions.Builders;

public interface ICommandExecutorBuilder
{
    ICommandApprovalFlowEventExecutor BuildCommandExecutor(CqrsContext context,
                                                           string commandFullName,
                                                           string responseFullName,
                                                           string commandHandlerFullName,
                                                           string approvalFlowHandlerFullName,
                                                           object command,
                                                           bool withApprovalFlow = false);

    ICommandApprovalFlowEventExecutor BuildCommandExecutor(CqrsContext context,
                                                           Type commandType,
                                                           Type responseType,
                                                           Type commandHandlerType,
                                                           Type approvalFlowHandlerType,
                                                           object command,
                                                           bool withApprovalFlow = false);
}

public interface ICommandExecutorDynamicBuilder
{
    ICommandExecutorDynamicBuilder WithServiceProvider(IServiceProvider services);

    ICommandExecutorDynamicBuilder WithCommand(object command);

    ICommandExecutorDynamicBuilder WithHandler(Type commandHandlerType);

    ICommandExecutorDynamicBuilder WithDefaultHandler();

    ICommandExecutorDynamicBuilder WithApprovalFlowHandler(Type approvalFlowHandlerType);

    ICommandExecutorDynamicBuilder WithDefaultApprovalFlowHandler();

    ICommandExecutorDynamicBuilder WithoutApprovalFlow();

    ICommandExecutorDynamicBuilder WithApprovalFlow();

    ICommandExecutorDynamicBuilder WithItems(IDictionary<object, object?> items);

    object Build();
}