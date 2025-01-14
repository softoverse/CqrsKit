using CqrsKit.Attributes;
using CqrsKit.Filters;
using CqrsKit.Model;
using CqrsKit.Model.Abstraction;
using CqrsKit.Model.Utility;
using CqrsKit.Services;

namespace CqrsKit.TestConsole.CQRS.Filters.Command;

[ScopedLifetime]
public class CommandExecutionFilter<TCommand, TResponse> : CommandExecutionFilterBase<TCommand, TResponse>
    where TCommand : ICommand
{
    public override Task<Response<TResponse>> OnExecutingAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutingAsync)}");
        return ResponseDefaults.DefaultResponse<TResponse>();
    }

    public override Task<Response<TResponse>> OnExecutedAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {UtilityHelper.GetFormattedTypeName(this.GetType())}.{nameof (this.OnExecutedAsync)}");
        return ResponseDefaults.DefaultResponse<TResponse>();
    }
}