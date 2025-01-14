using CqrsKit.Executors;

using CqrsKit.Abstraction.Builders;
using CqrsKit.Abstraction.Executors;
using CqrsKit.Abstraction.Filters;
using CqrsKit.Abstraction.Handlers;

using CqrsKit.Extensions;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;

using Microsoft.Extensions.DependencyInjection;

namespace CqrsKit.Builders;

internal sealed class QueryExecutorBuilder<TQuery, TResponse> : IQueryExecutorBuilder<TQuery, TResponse>
    where TQuery : IQuery
{
    private readonly IServiceProvider _services;
    private TQuery _query;
    private IQueryHandler<TQuery, TResponse>? _queryHandler;

    private readonly CqrsContext _context = CqrsContext.New();

    private QueryExecutorBuilder() { }

    private QueryExecutorBuilder(IServiceProvider serviceProvider)
    {
        _services = serviceProvider;
    }

    public static IQueryExecutorBuilder<TQuery, TResponse> Initialize(IServiceProvider services) => new QueryExecutorBuilder<TQuery, TResponse>(services);

    public IQueryExecutorBuilder<TQuery, TResponse> WithQuery(TQuery query)
    {
        _query = query;
        ArgumentNullException.ThrowIfNull(query);
        return this;
    }

    public IQueryExecutorBuilder<TQuery, TResponse> WithHandler<THandler>() where THandler : IQueryHandler<TQuery, TResponse>
    {
        _queryHandler = _services.GetService<THandler>()!;
        if (_queryHandler == null) throw new Exception($"Handler {typeof(THandler).Name} not found in dependency injection.");
        _context.IsCustomQueryHandler = true;
        return this;
    }

    public IQueryExecutorBuilder<TQuery, TResponse> WithHandler(Type queryHandlerType)
    {
        ArgumentNullException.ThrowIfNull(queryHandlerType);

        _queryHandler = _services.GetService(queryHandlerType) as IQueryHandler<TQuery, TResponse> ?? throw new Exception($"Handler {queryHandlerType.Name} not found in dependency injection.");
        _context.IsCustomQueryHandler = true;
        return this;
    }

    public IQueryExecutorBuilder<TQuery, TResponse> WithDefaultHandler()
    {
        _queryHandler = _services.GetQueryHandler<TQuery, TResponse>() ?? throw new Exception($"Handler for {typeof(TQuery).Name} not found in dependency injection.");
        _context.IsCustomQueryHandler = false;
        return this;
    }

    public IQueryExecutorBuilder<TQuery, TResponse> WithItems(IDictionary<object, object?> items)
    {
        _context.Items = items;
        return this;
    }

    public IQueryExecutor<TQuery, TResponse> Build()
    {
        var executionFilter = _services.GetRequiredService<IExecutionFilter<TQuery, TResponse>>();

        if (_queryHandler == null) WithDefaultHandler();

        return new QueryExecutor<TQuery, TResponse>
        {
            Services = _services,
            Query = _query,
            QueryHandler = _queryHandler!,
            ExecutionFilter = executionFilter,
            Context = _context,
        };
    }
}