using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class ExecutionFilterAttribute : Attribute,
                                                 IAsyncExecutionFilter
{
    public virtual Task OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}