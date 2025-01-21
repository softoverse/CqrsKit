﻿using System.Collections.Concurrent;

using Softoverse.CqrsKit.Model.Abstraction;

namespace Softoverse.CqrsKit.Model.Utility;

public class CqrsContext
{
    public IRequest Request { get; set; }
    public object? Response { get; set; }
    
    public string ApprovalFlowPendingTaskId { get; set; }

    public CurrentState State { get; set; } = CurrentState.None;

    private IDictionary<object, object?> _items = new ConcurrentDictionary<object, object?>();

    public IDictionary<object, object?> Items
    {
        get
        {
            return _items ?? new ConcurrentDictionary<object, object?>();
        }

        set
        {
            _items = value;
        }
    }

    public bool IsCustomCommandHandler { get; set; }
    public bool IsCustomQueryHandler { get; set; }
    public bool IsApprovalFlowEnabled { get; set; }
    public bool IsCustomApprovalFlowHandler { get; set; }

    public string RequestName { get; set; }
    public string RequestFullName { get; set; }
    public string RequestNamespace { get; set; }

    public string ResponseName { get; set; }
    public string ResponseFullName { get; set; }
    public string ResponseNamespace { get; set; }

    public string HandlerName { get; set; }
    public string HandlerFullName { get; set; }
    public string HandlerNamespace { get; set; }

    public string ApprovalFlowHandlerName { get; set; }
    public string ApprovalFlowHandlerFullName { get; set; }
    public string ApprovalFlowHandlerNamespace { get; set; }

    public T? GetItem<T>(string key)
    {
        return Items.TryGetValue(key, out object? value)
            ? (T)value!
            : default;
    }

    public T RequestAs<T>() where T : IRequest
    {
        return (T)Request!;
    }
    
    public T ResponseAs<T>()
    {
        return (T)Response!;
    }

    public static CqrsContext New() => new CqrsContext();
}