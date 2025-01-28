using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

using Softoverse.CqrsKit.Abstraction.Builders;
using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.Builders;

internal sealed class CommandExecutorDynamicBuilder : ICommandExecutorBuilder
{
    private static readonly Type CommandExecutorBuilderWithReturnType = typeof(CommandExecutorBuilder<,>);
    private static readonly ConcurrentDictionary<(Type, Type), Func<object, ICommandExecutorDynamicBuilder>> BuilderCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), (ConstructorInfo, Type)> CommandExecutorBuilderConstructor = new();

    private readonly IServiceProvider _services;

    private CommandExecutorDynamicBuilder()
    {
    }

    private CommandExecutorDynamicBuilder(IServiceProvider services)
    {
        _services = services;
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
        // Validate input types and ensure they implement the required interfaces
        if (!typeof(ICommand).IsAssignableFrom(commandType))
            throw new ArgumentException($"{commandType} must implement {nameof (ICommand)}.", nameof (commandType));

        if (!typeof(ICommandHandler<,>).MakeGenericType(commandType, responseType).IsAssignableFrom(commandHandlerType))
            throw new ArgumentException($"{commandHandlerType} must implement ICommandHandler<{commandType.Name}, {responseType.Name}>.", nameof (commandHandlerType));

        if (withApprovalFlow && !typeof(IApprovalFlowHandler<,>).IsAssignableFrom(approvalFlowHandlerType))
            throw new ArgumentException($"{approvalFlowHandlerType} must implement IApprovalFlowHandler.", nameof (approvalFlowHandlerType));


        #region using Expression Tree

        // Stopwatch sw1 = Stopwatch.StartNew();
        // // Retrieve or create the factory delegate for CommandExecutorBuilder
        // var builderFactory = BuilderCache.GetOrAdd((commandType, responseType), CreateBuilderFactory);
        //
        // // Create the CommandExecutorBuilder instance
        // var commandExecutorBuilderWithExpressionTree = builderFactory(_services);
        // var commandExecutorBuilder = commandExecutorBuilderWithExpressionTree;
        // sw1.Stop();

        #endregion using Expression Tree

        #region using reflection

        // Stopwatch sw2 = Stopwatch.StartNew();
        (ConstructorInfo ConstructorInfo, Type BuilderType) commandExecutorBuilderConstructor = CommandExecutorBuilderConstructor.GetOrAdd((commandType, responseType), CreateCommandExecutorBuilderConstructorInfo);
        var commandExecutorBuilderWithReflection = (ICommandExecutorDynamicBuilder)commandExecutorBuilderConstructor.ConstructorInfo.Invoke([_services]);
        var commandExecutorBuilder = commandExecutorBuilderWithReflection;
        // sw2.Stop();

        #endregion using reflection

        // Consolidate chained method calls to reduce overhead
        commandExecutorBuilder.WithServiceProvider(_services)
                              .WithCommand(command)
                              .WithHandler(commandHandlerType)
                              .WithItems(context.Items);

        if (withApprovalFlow)
            commandExecutorBuilder.WithApprovalFlow()
                                  .WithApprovalFlowHandler(approvalFlowHandlerType);
        else
            commandExecutorBuilder.WithoutApprovalFlow();

        var result = commandExecutorBuilder.Build();

        return result as ICommandApprovalFlowEventExecutor
            ?? throw new InvalidOperationException($"Build process failed to return '{nameof (ICommandApprovalFlowEventExecutor)}'.");
    }

    private static Func<object, ICommandExecutorDynamicBuilder> CreateBuilderFactory((Type commandType, Type responseType) types)
    {
        // Precompute the generic type and validate it
        var builderType = CommandExecutorBuilderWithReturnType.MakeGenericType(types.commandType, types.responseType);
        var constructor = builderType.GetConstructor([typeof(IServiceProvider)])
                       ?? throw new InvalidOperationException($"Constructor not found for {builderType}.");

        // Build the delegate more efficiently
        var serviceProviderParam = Expression.Parameter(typeof(object), "services");
        var constructorCall = Expression.New(constructor, Expression.Convert(serviceProviderParam, typeof(IServiceProvider)));

        // Eliminate intermediate Expression.Convert if unnecessary
        var lambda = Expression.Lambda<Func<object, ICommandExecutorDynamicBuilder>>(
                                                                                     Expression.Convert(constructorCall, typeof(ICommandExecutorDynamicBuilder)),
                                                                                     serviceProviderParam);

        return lambda.Compile();
    }

    private static (ConstructorInfo ConstructorInfo, Type BuilderType) CreateCommandExecutorBuilderConstructorInfo((Type CommandType, Type ResponseType) types)
    {
        // Create the generic CommandExecutorBuilder type
        Type builderType = CommandExecutorBuilderWithReturnType.MakeGenericType(types.CommandType, types.ResponseType);

        ConstructorInfo constructor = builderType.GetConstructor([typeof(IServiceProvider)])
                                   ?? throw new InvalidOperationException($"Constructor not found for {builderType}.");

        return (constructor, builderType);

        // // Cache the method lookup to avoid repetitive reflection overhead
        // var initializeMethod = builderType.GetMethod(nameof (CommandExecutorBuilder<ICommand, object>.Initialize), BindingFlags.Public | BindingFlags.Static)
        //                     ?? throw new InvalidOperationException($"Could not find constructor of '{nameof (CommandExecutorDynamicBuilder)}'.");
        //
        //
        //
        // // Use a direct cast instead of repeated dynamic casting
        // if (initializeMethod.Invoke(null, [services]) is not ICommandExecutorDynamicBuilder commandExecutorBuilderInstance)
        // {
        //     throw new InvalidOperationException($"Failed to create an instance of {nameof (ICommandExecutorDynamicBuilder)}.");
        // }
        //
        // return commandExecutorBuilderInstance;
    }
}