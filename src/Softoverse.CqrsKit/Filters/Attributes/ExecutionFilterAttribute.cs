using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public abstract class ExecutionFilterAttribute : Attribute,
                                                 IAsyncExecutionFilter
{

    public virtual Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }

    internal IAsyncExecutionFilter AsyncExecutionFilter
    {
        get
        {
            return (IAsyncExecutionFilter)this;
        }
    }
}