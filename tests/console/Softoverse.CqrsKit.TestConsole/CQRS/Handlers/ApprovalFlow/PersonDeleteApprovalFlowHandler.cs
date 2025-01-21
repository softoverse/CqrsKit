using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.ApprovalFlow;

[TransientLifetime]
public class PersonDeleteApprovalFlowHandler : ApprovalFlowHandler<PersonDeleteCommand, Guid>
{
    public override async Task<Result<Guid>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        context.Items.Add("Test", "TEST");
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Before Approval Flow Start Person")
                                                   .WithPayload(command.Payload));
    }
    
    public override async Task<Result<Guid>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After Approval Flow End Person")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> AfterAcceptAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterAcceptAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("After Approval Flow Accept Person")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> AfterRejectAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterRejectAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("After Approval Flow Reject Person")
                                                   .WithPayload(command.Payload));
    }
}