using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowAcceptFilter : ApprovalFlowAcceptFilterBase
{
    public override Task<Result> OnAcceptingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnAcceptingAsync)}");

        return ResponseDefaults.DefaultResponse();
    }

    public override Task<Result> OnAcceptedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnAcceptedAsync)}");
        return ResponseDefaults.DefaultResponse();
    }
}