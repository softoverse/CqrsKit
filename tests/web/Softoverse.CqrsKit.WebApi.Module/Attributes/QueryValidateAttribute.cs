using Softoverse.CqrsKit.Filters.Attributes;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.WebApi.Module.Attributes;

public class QueryAuthorizeAttribute : ExecutionFilterAttribute
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