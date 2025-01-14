using System.Reflection;

using CqrsKit.Services;

using CqrsKit.Abstraction.Builders;
using CqrsKit.Abstraction.Executors;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

namespace CqrsKit.Builders;

internal sealed class CommandExecutorDynamicBuilder : ICommandExecutorBuilder
{
    private static readonly Type CommandExecutorBuilderWithReturnType = typeof(CommandExecutorBuilder<,>);

    private readonly IServiceProvider _services;

    private CommandExecutorDynamicBuilder()
    {
    }

    private CommandExecutorDynamicBuilder(IServiceProvider serviceProvider)
    {
        _services = serviceProvider;
    }

    public static ICommandExecutorBuilder Initialize(IServiceProvider services)
    {
        return new CommandExecutorDynamicBuilder(services);
    }

    /// <summary>
    /// Builds a command executor with a specified response type.
    /// </summary>
    public ICommandApprovalFlowEventExecutor BuildCommandExecutor(
        CqrsContext context,
        string commandFullName,
        string responseFullName,
        string commandHandlerFullName,
        string approvalFlowHandlerFullName,
        object command,
        bool withApprovalFlow = false)
    {
        Type commandType = CqrsHelper.GetType(commandFullName);
        Type responseType = CqrsHelper.GetType(responseFullName);
        Type commandHandlerType = CqrsHelper.GetType(commandHandlerFullName);
        Type approvalFlowHandlerType = CqrsHelper.GetType(approvalFlowHandlerFullName);

        return BuildCommandExecutor(context, commandType, responseType, commandHandlerType, approvalFlowHandlerType, command, withApprovalFlow);
    }

    /// <summary>
    /// Internal method to build a command executor using reflection.
    /// </summary>
    public ICommandApprovalFlowEventExecutor BuildCommandExecutor(CqrsContext context,
                                                                  Type commandType,
                                                                  Type responseType,
                                                                  Type commandHandlerType,
                                                                  Type approvalFlowHandlerType,
                                                                  object command,
                                                                  bool withApprovalFlow)
    {
        // Create the generic CommandExecutorBuilder type
        Type builderGenericType = CommandExecutorBuilderWithReturnType.MakeGenericType(commandType, responseType);

        // Cache the method lookup to avoid repetitive reflection overhead
        var initializeMethod = builderGenericType.GetMethod(nameof (CommandExecutorBuilder<ICommand, object>.Initialize), BindingFlags.Public | BindingFlags.Static)
                            ?? throw new InvalidOperationException($"Could not find constructor of '{nameof (CommandExecutorDynamicBuilder)}'.");

        // Use a direct cast instead of repeated dynamic casting
        if (initializeMethod.Invoke(null, [_services]) is not ICommandExecutorDynamicBuilder commandExecutorBuilderInstance)
        {
            throw new InvalidOperationException($"Failed to create an instance of {nameof (ICommandExecutorDynamicBuilder)}.");
        }

        // Consolidate chained method calls to reduce overhead
        commandExecutorBuilderInstance.WithServiceProvider(_services)
                                      .WithCommand(command)
                                      .WithHandler(commandHandlerType)
                                      .WithItems(context.Items);

        if (withApprovalFlow)
            commandExecutorBuilderInstance.WithApprovalFlow().WithApprovalFlowHandler(approvalFlowHandlerType);
        else
            commandExecutorBuilderInstance.WithoutApprovalFlow();

        // Cache the result of the final Build call
        var result = commandExecutorBuilderInstance.Build();

        return result as ICommandApprovalFlowEventExecutor
            ?? throw new InvalidOperationException($"Build process failed to return '{nameof (ICommandApprovalFlowEventExecutor)}'.");
    }
}