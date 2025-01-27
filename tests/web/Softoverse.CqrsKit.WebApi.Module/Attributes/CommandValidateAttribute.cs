using Softoverse.CqrsKit.Filters.Attributes;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.WebApi.Module.Attributes;

public class CommandAuthorizeAttribute : ExecutionFilterAttribute
{
    public override Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        return base.OnActionExecutingAsync(context, ct);
    }

    public override Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        return base.OnActionExecutedAsync(context, ct);
    }
}