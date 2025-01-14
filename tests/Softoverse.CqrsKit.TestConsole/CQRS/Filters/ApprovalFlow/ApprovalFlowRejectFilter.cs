using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowRejectFilter : ApprovalFlowRejectFilterBase
{
    public override Task<Response> OnRejectingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnRejectingAsync)}");

        return ResponseDefaults.DefaultResponse();
    }

    public override Task<Response> OnRejectedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnRejectedAsync)}");
        return ResponseDefaults.DefaultResponse();
    }
}