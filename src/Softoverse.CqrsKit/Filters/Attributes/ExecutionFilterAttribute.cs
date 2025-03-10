﻿using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.Filters.Attributes;

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