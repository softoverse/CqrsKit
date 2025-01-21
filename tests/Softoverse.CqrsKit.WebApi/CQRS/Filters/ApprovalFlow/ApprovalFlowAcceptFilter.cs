using Softoverse.CqrsKit.Abstraction.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.WebApi.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowAcceptFilter : ApprovalFlowAcceptFilterBase
{
    public override Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnExecutingAsync)}");

        return ResponseDefaults.DefaultResponse();
    }

    public override Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnExecutedAsync)}");
        return ResponseDefaults.DefaultResponse();
    }
}