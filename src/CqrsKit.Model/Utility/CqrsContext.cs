using System.Collections.Concurrent;

namespace CqrsKit.Model.Utility;

public class CqrsContext
{
    public object? Request { get; set; }
    public object? Response { get; set; }

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

    public string CommandName { get; set; }
    public string CommandFullName { get; set; }
    public string CommandNamespace { get; set; }

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

    public static CqrsContext New() => new CqrsContext();
}