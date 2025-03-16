using Softoverse.CqrsKit.Abstractions.Filters;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Abstraction;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.Services;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowExecutionFilter<TCommand, TResponse> : ApprovalFlowExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Result<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)}");
        return ResultDefaults.DefaultResult<TResponse>();
    }

    public override Task<Result<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.ExecuteAsync)}");
        return ResultDefaults.DefaultResult<TResponse>();
    }

    public override Task<Result<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)}");
        return ResultDefaults.DefaultResult<TResponse>();
    }
}