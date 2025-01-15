using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Filters;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class StudentDeleteCommandApprovalFlowExecutionFilter : ApprovalFlowExecutionFilterBase<StudentDeleteCommand, Guid>
{
    public override Task<Result<Guid>> OnExecutingAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<Guid>();
    }

    public override Task<Result<Guid>> ExecuteAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.ExecuteAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<Guid>();
    }

    public override Task<Result<Guid>> OnExecutedAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<Guid>();
    }
}