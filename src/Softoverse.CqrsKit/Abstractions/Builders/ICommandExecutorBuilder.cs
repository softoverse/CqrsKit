﻿using Softoverse.CqrsKit.Abstractions.Executors;
using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.Abstractions.Builders;

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