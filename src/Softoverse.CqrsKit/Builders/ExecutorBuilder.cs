using Softoverse.CqrsKit.Abstraction.Builders;
using Softoverse.CqrsKit.Model.Abstraction;
using Softoverse.CqrsKit.Model.Entity;

namespace Softoverse.CqrsKit.Builders;

internal static class QueryBuilder
{
    public static IQueryExecutorBuilder<TQuery, TResponse> Initialize<TQuery, TResponse>(this IServiceProvider services) where TQuery : IQuery
    {
        return QueryExecutorBuilder<TQuery, TResponse>.Initialize(services);
    }
}

internal static class CommandBuilder
{
    public static ICommandExecutorBuilder<TCommand, TResponse> Initialize<TCommand, TResponse>(this IServiceProvider services) where TCommand : ICommand
    {
        return CommandExecutorBuilder<TCommand, TResponse>.Initialize(services);
    }
}

internal static class ApprovalFlowBuilder
{
    public static IApprovalFlowExecutorBuilder<T> Initialize<T>(this IServiceProvider services) where T : BaseApprovalFlowPendingTask
    {
        return ApprovalFlowExecutorBuilder<T>.Initialize(services);
    }
}

public static class CqrsBuilder
{
    public static IQueryExecutorBuilder<TQuery, TResponse> Query<TQuery, TResponse>(IServiceProvider services) where TQuery : IQuery
    {
        return services.Initialize<TQuery, TResponse>();
    }

    public static ICommandExecutorBuilder<TCommand, TResponse> Command<TCommand, TResponse>(IServiceProvider services) where TCommand : ICommand
    {
        return services.Initialize<TCommand, TResponse>();
    }

    public static IApprovalFlowExecutorBuilder<T> ApprovalFlow<T>(IServiceProvider services) where T : BaseApprovalFlowPendingTask
    {
        return services.Initialize<T>();
    }
}