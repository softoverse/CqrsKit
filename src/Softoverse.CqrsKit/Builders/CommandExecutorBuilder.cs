using Microsoft.Extensions.DependencyInjection;

using Softoverse.CqrsKit.Abstractions.Builders;
using Softoverse.CqrsKit.Abstractions.Executors;
using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Abstractions.Services;
using Softoverse.CqrsKit.Executors;
using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.Models.Abstraction;

namespace Softoverse.CqrsKit.Builders;

internal sealed class CommandExecutorBuilder<TCommand, TResponse> : CommandExecutorBuilderBase<TCommand>,
                                                                    ICommandExecutorBuilder<TCommand, TResponse>
    where TCommand : ICommand
{
    private ICommandHandler<TCommand, TResponse>? _commandHandler;
    private IApprovalFlowHandler<TCommand, TResponse>? _approvalFlowHandler;

    private CommandExecutorBuilder(): base() { }

    public CommandExecutorBuilder(IServiceProvider services) : base(services) { }

    public static ICommandExecutorBuilder<TCommand, TResponse> Initialize(IServiceProvider services) => new CommandExecutorBuilder<TCommand, TResponse>(services);

    #region Private Methods

    private ICommandExecutorBuilder<TCommand, TResponse> WithHandlerBase(Type commandHandlerType)
    {
        ArgumentNullException.ThrowIfNull(commandHandlerType);

        _commandHandler = _services.GetService(commandHandlerType) as ICommandHandler<TCommand, TResponse> ?? throw new Exception($"Handler {commandHandlerType.Name} not found in dependency injection.");
        _context.IsCustomCommandHandler = true;
        return this;
    }

    private ICommandExecutorBuilder<TCommand, TResponse> WithDefaultHandlerBase()
    {
        _commandHandler = _services.GetCommandHandler<TCommand, TResponse>() ?? throw new Exception($"Handler for {typeof(TCommand).Name} not found in dependency injection.");
        _context.IsCustomCommandHandler = false;
        return this;
    }

    private ICommandExecutorBuilder<TCommand, TResponse> WithApprovalFlowHandlerBase(Type approvalFlowHandlerType)
    {
        ArgumentNullException.ThrowIfNull(approvalFlowHandlerType);

        _approvalFlowHandler = _services.GetService(approvalFlowHandlerType) as IApprovalFlowHandler<TCommand, TResponse>;
        if (_approvalFlowHandler == null)
        {
            _isApprovalFlowEnabled = false;
        }

        _context.IsCustomApprovalFlowHandler = true;
        return this;
    }

    private ICommandExecutorBuilder<TCommand, TResponse> WithDefaultApprovalFlowHandlerBase()
    {
        _approvalFlowHandler = _services.GetApprovalFlowHandler<TCommand, TResponse>();
        if (_approvalFlowHandler == null)
        {
            _isApprovalFlowEnabled = false;
        }

        _context.IsCustomApprovalFlowHandler = false;
        return this;
    }

    private ICommandExecutor<TCommand, TResponse> BuildBase()
    {
        var executionFilter = _services.GetRequiredService<IExecutionFilter<TCommand, TResponse>>();
        var asyncExecutionFilter = _services.GetService<IAsyncExecutionFilter<TCommand, TResponse>>();
        var approvalFlowExecutionFilter = _services.GetRequiredService<IApprovalFlowExecutionFilter<TCommand, TResponse>>();
        var approvalFlowService = _services.GetRequiredService<IApprovalFlowService>();

        if (_commandHandler == null) WithDefaultHandler();
        if (_isApprovalFlowEnabled && _approvalFlowHandler == null) WithDefaultApprovalFlowHandler();

        return new CommandExecutor<TCommand, TResponse>
        {
            Services = _services,
            Command = _command,
            CommandHandler = _commandHandler!,
            IsApprovalFlowEnabled = _isApprovalFlowEnabled,
            ExecutionFilter = executionFilter,
            AsyncExecutionFilter = asyncExecutionFilter,
            ApprovalFlowExecutionFilter = approvalFlowExecutionFilter,
            ApprovalFlowService = approvalFlowService,
            ApprovalFlowHandler = _approvalFlowHandler,
            Context = _context,
        };
    }

    #endregion Private Methods

    #region CommandExecutorDynamicBuilderBase<TCommand>

    public override ICommandExecutorDynamicBuilder WithHandler(Type commandHandlerType) => (ICommandExecutorDynamicBuilder) WithHandlerBase(commandHandlerType);

    public override ICommandExecutorDynamicBuilder WithDefaultHandler() => (ICommandExecutorDynamicBuilder) WithDefaultHandlerBase();

    public override ICommandExecutorDynamicBuilder WithApprovalFlowHandler(Type approvalFlowHandlerType) => (ICommandExecutorDynamicBuilder) WithApprovalFlowHandlerBase(approvalFlowHandlerType);

    public override ICommandExecutorDynamicBuilder WithDefaultApprovalFlowHandler() => (ICommandExecutorDynamicBuilder) WithDefaultApprovalFlowHandlerBase();

    public override object Build() => BuildBase();

    #endregion CommandExecutorDynamicBuilderBase<TCommand>

    #region Different Method Calls

    public ICommandExecutorBuilder<TCommand, TResponse> WithHandler<THandler>() where THandler : ICommandHandler<TCommand, TResponse>
    {
        _commandHandler = _services.GetService<THandler>() ?? throw new Exception($"Handler {typeof(THandler).Name} not found in dependency injection.");
        _context.IsCustomCommandHandler = true;
        return this;
    }

    public ICommandExecutorBuilder<TCommand, TResponse> WithApprovalFlowHandler<THandler>() where THandler : IApprovalFlowHandler<TCommand, TResponse>
    {
        _approvalFlowHandler = _services.GetService<THandler>()!;
        if (_approvalFlowHandler == null)
        {
            _isApprovalFlowEnabled = false;
        }

        _context.IsCustomApprovalFlowHandler = true;
        return this;
    }

    #endregion Different Method Calls

    #region Base Class's Method Calls

    public ICommandExecutorBuilder<TCommand, TResponse> WithCommand(TCommand command)
    {
        base.WithCommand(command);
        return this;
    }

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithApprovalFlow()
    {
        base.WithApprovalFlow();
        return this;
    }

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithoutApprovalFlow()
    {
        base.WithoutApprovalFlow();
        return this;
    }

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithItems(IDictionary<object, object?> items)
    {
        base.WithItems(items);
        return this;
    }

    #endregion Base Class's Method Calls

    #region Private Base Method Calls

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithHandler(Type commandHandlerType)
    {
        return WithHandlerBase(commandHandlerType);
    }

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithDefaultHandler()
    {
        return WithDefaultHandlerBase();
    }

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithApprovalFlowHandler(Type approvalFlowHandlerType)
    {
        return WithApprovalFlowHandlerBase(approvalFlowHandlerType);
    }

    ICommandExecutorBuilder<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.WithDefaultApprovalFlowHandler()
    {
        return WithDefaultApprovalFlowHandlerBase();
    }

    ICommandExecutor<TCommand, TResponse> ICommandExecutorBuilder<TCommand, TResponse>.Build()
    {
        return BuildBase();
    }

    #endregion Private Base Method Calls
}