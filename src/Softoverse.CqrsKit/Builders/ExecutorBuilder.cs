using Softoverse.CqrsKit.Abstraction.Builders;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Entity;

namespace Softoverse.CqrsKit.Builders;

public static class QueryBuilder
{
    public static IQueryExecutorBuilder<TQuery, TResponse> Initialize<TQuery, TResponse>(IServiceProvider services) where TQuery : IQuery
    {
        return QueryExecutorBuilder<TQuery, TResponse>.Initialize(services);
    }
}

public static class CommandBuilder
{
    public static ICommandExecutorBuilder<TCommand, TResponse> Initialize<TCommand, TResponse>(IServiceProvider services) where TCommand : ICommand
    {
        return CommandExecutorBuilder<TCommand, TResponse>.Initialize(services);
    }
}

public static class ApprovalFlowBuilder
{
    public static IApprovalFlowExecutorBuilder<T> Initialize<T>(IServiceProvider services) where T : BaseApprovalFlowPendingTask
    {
        return ApprovalFlowExecutorBuilder<T>.Initialize(services);
    }
}

public static class CqrsBuilder
{
    public static IQueryExecutorBuilder<TQuery, TResponse> Query<TQuery, TResponse>(IServiceProvider services) where TQuery : IQuery
    {
        return QueryBuilder.Initialize<TQuery, TResponse>(services);
    }
    
    public static ICommandExecutorBuilder<TCommand, TResponse> Command<TCommand, TResponse>(IServiceProvider services) where TCommand : ICommand
    {
        return CommandBuilder.Initialize<TCommand, TResponse>(services);
    }
    
    public static IApprovalFlowExecutorBuilder<T> ApprovalFlow<T>(IServiceProvider services) where T : BaseApprovalFlowPendingTask
    {
        return ApprovalFlowBuilder.Initialize<T>(services);
    }
}