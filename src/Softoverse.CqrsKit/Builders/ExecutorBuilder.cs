using Softoverse.CqrsKit.Abstractions.Builders;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Entity;

namespace Softoverse.CqrsKit.Builders;

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