using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;
using CqrsKit.Services;

namespace CqrsKit.TestConsole.CQRS.Filters.ApprovalFlow;

[ScopedLifetime]
public class ApprovalFlowExecutionFilter<TCommand, TResponse> : ApprovalFlowExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Response<TResponse>> OnExecutingAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)}");
        return ResponseDefaults.DefaultResponse<TResponse>();
    }

    public override Task<Response<TResponse>> ExecuteAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.ExecuteAsync)}");
        return ResponseDefaults.DefaultResponse<TResponse>();
    }

    public override Task<Response<TResponse>> OnExecutedAsync(TCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)}");
        return ResponseDefaults.DefaultResponse<TResponse>();
    }
}