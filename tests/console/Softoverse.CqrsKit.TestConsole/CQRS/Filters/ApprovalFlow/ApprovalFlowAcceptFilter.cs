using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowAcceptFilter : ApprovalFlowAcceptFilterBase
{
    public override Task<Result> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnExecutingAsync)}");

        return ResultDefaults.DefaultResult();
    }

    public override Task<Result> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnExecutedAsync)}");
        return ResultDefaults.DefaultResult();
    }
}