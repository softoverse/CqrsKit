using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Attributes;

public class QueryAuthorizeAttribute : ExecutionFilterAttribute
{
    public override Task OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnExecutingAsync)}");
        return base.OnExecutingAsync(context, ct);
    }

    public override Task OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnExecutedAsync)}");
        return base.OnExecutedAsync(context, ct);
    }
}