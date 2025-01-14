using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.Abstraction.Builders;

public interface ICommandExecutorBuilder<TCommand, TResponse> where TCommand : ICommand
{
    ICommandExecutorBuilder<TCommand, TResponse> WithCommand(TCommand command);

    ICommandExecutorBuilder<TCommand, TResponse> WithHandler<THandler>() where THandler : ICommandHandler<TCommand, TResponse>;

    ICommandExecutorBuilder<TCommand, TResponse> WithHandler(Type commandHandlerType);

    ICommandExecutorBuilder<TCommand, TResponse> WithDefaultHandler();

    ICommandExecutorBuilder<TCommand, TResponse> WithApprovalFlowHandler<THandler>() where THandler : IApprovalFlowHandler<TCommand, TResponse>;

    ICommandExecutorBuilder<TCommand, TResponse> WithApprovalFlowHandler(Type approvalFlowHandlerType);

    ICommandExecutorBuilder<TCommand, TResponse> WithDefaultApprovalFlowHandler();

    ICommandExecutorBuilder<TCommand, TResponse> WithoutApprovalFlow();

    ICommandExecutorBuilder<TCommand, TResponse> WithApprovalFlow();

    ICommandExecutorBuilder<TCommand, TResponse> WithItems(IDictionary<object, object?> items);

    ICommandExecutor<TCommand, TResponse> Build();
}