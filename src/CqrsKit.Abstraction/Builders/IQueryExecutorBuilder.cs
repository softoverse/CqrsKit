using CqrsKit.Abstraction.Executors;
using CqrsKit.Abstraction.Handlers;

using CqrsKit.Model.Abstraction;

namespace CqrsKit.Abstraction.Builders;

public interface IQueryExecutorBuilder<TQuery, TResponse> where TQuery : IQuery
{
    IQueryExecutorBuilder<TQuery, TResponse> WithQuery(TQuery command);

    IQueryExecutorBuilder<TQuery, TResponse> WithHandler<THandler>() where THandler : IQueryHandler<TQuery, TResponse>;

    IQueryExecutorBuilder<TQuery, TResponse> WithDefaultHandler();

    IQueryExecutorBuilder<TQuery, TResponse> WithItems(IDictionary<object, object?> items);

    IQueryExecutor<TQuery, TResponse> Build();
}