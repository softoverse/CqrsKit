using Softoverse.CqrsKit.Abstraction.Executors;
using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.Abstraction.Builders;

public interface IQueryExecutorBuilder<TQuery, TResponse> where TQuery : IQuery
{
    IQueryExecutorBuilder<TQuery, TResponse> WithQuery(TQuery command);

    IQueryExecutorBuilder<TQuery, TResponse> WithHandler<THandler>() where THandler : IQueryHandler<TQuery, TResponse>;

    IQueryExecutorBuilder<TQuery, TResponse> WithDefaultHandler();

    IQueryExecutorBuilder<TQuery, TResponse> WithItems(IDictionary<object, object?> items);

    IQueryExecutor<TQuery, TResponse> Build();
}