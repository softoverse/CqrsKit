using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Utility;
using CqrsKit.Services;
using CqrsKit.TestConsole.CQRS.Events.Command;

namespace CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class StudentDeleteCommandApprovalFlowExecutionFilter : ApprovalFlowExecutionFilterBase<StudentDeleteCommand, Guid>
{
    public override Task<Response<Guid>> OnExecutingAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<Guid>();
    }

    public override Task<Response<Guid>> ExecuteAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.ExecuteAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<Guid>();
    }

    public override Task<Response<Guid>> OnExecutedAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)} - (Custom)");
        return ResponseDefaults.DefaultResponse<Guid>();
    }
}