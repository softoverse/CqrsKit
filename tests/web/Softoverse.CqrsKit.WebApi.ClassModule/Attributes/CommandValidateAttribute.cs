﻿using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.WebApi.Module.Attributes;

public class CommandAuthorizeAttribute : ExecutionFilterAttribute
{
    public override Task OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return base.OnExecutingAsync(context, ct);
    }

    public override Task OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return base.OnExecutedAsync(context, ct);
    }
}