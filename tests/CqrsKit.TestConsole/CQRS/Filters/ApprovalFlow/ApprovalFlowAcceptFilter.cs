using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Utility;

namespace CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowAcceptFilter : ApprovalFlowAcceptFilterBase
{
    public override Task<Response> OnAcceptingAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnAcceptingAsync)}");

        return ResponseDefaults.DefaultResponse();
    }

    public override Task<Response> OnAcceptedAsync(string approvalFlowId, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnAcceptedAsync)}");
        return ResponseDefaults.DefaultResponse();
    }
}