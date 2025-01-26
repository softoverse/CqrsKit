using Softoverse.CqrsKit.Filters.Attributes;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Attributes;

public class CommandAuthorizeAttribute : ExecutionFilterAttribute
{
    public override Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnActionExecutingAsync)}");
        return base.OnActionExecutingAsync(context, ct);
    }

    public override Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnActionExecutedAsync)}");
        return base.OnActionExecutedAsync(context, ct);
    }
}

public class CommandValidateAttribute : ExecutionFilterAttribute
{
    public override Task OnActionExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnActionExecutingAsync)}");
        return base.OnActionExecutingAsync(context, ct);
    }

    public override Task OnActionExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnActionExecutedAsync)}");
        return base.OnActionExecutedAsync(context, ct);
    }
}