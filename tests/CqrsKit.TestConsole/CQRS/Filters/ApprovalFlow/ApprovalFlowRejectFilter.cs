using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Utility;

namespace CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

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