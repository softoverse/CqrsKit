using Softoverse.CqrsKit.Abstractions.Builders;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Builders;

internal abstract class CommandExecutorBuilderBase<TCommand> : ICommandExecutorDynamicBuilder
    where TCommand : ICommand
{
    protected IServiceProvider _services;
    protected TCommand _command;
    protected bool _isApprovalFlowEnabled = false;

    protected readonly CqrsContext _context;

    protected CommandExecutorBuilderBase()
    {
        _context = CqrsContext.New();
        _context.IsApprovalFlowEnabled = _isApprovalFlowEnabled;
    }

    protected CommandExecutorBuilderBase(IServiceProvider services)
    {
        _services = services;
        _context = CqrsContext.New(_services);
        _context.IsApprovalFlowEnabled = _isApprovalFlowEnabled;
    }

    public ICommandExecutorDynamicBuilder WithServiceProvider(IServiceProvider services)
    {
        _services = services ?? throw new ArgumentNullException(nameof (services));
        return this;
    }

    public ICommandExecutorDynamicBuilder WithCommand(object command)
    {
        ArgumentNullException.ThrowIfNull(command);
        _command = (TCommand)command;
        return this;
    }

    public ICommandExecutorDynamicBuilder WithoutApprovalFlow()
    {
        _isApprovalFlowEnabled = false;
        _context.IsApprovalFlowEnabled = false;
        return this;
    }

    public ICommandExecutorDynamicBuilder WithApprovalFlow()
    {
        _isApprovalFlowEnabled = true;
        _context.IsApprovalFlowEnabled = true;
        return this;
    }

    public ICommandExecutorDynamicBuilder WithItems(IDictionary<object, object?> items)
    {
        _context.Items = items;
        return this;
    }

    public abstract ICommandExecutorDynamicBuilder WithHandler(Type commandHandlerType);

    public abstract ICommandExecutorDynamicBuilder WithDefaultHandler();

    public abstract ICommandExecutorDynamicBuilder WithApprovalFlowHandler(Type approvalFlowHandlerType);

    public abstract ICommandExecutorDynamicBuilder WithDefaultApprovalFlowHandler();

    public abstract object Build();
}