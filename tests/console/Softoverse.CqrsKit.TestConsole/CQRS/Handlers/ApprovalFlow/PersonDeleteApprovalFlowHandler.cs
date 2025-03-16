using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.ApprovalFlow;

[TransientLifetime]
public class PersonDeleteApprovalFlowHandler : ApprovalFlowHandler<PersonDeleteCommand, Guid>
{
    public override async Task<Result<Guid>> OnStartAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        context.Items.Add("Test", "TEST");
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Before Approval Flow Start Person")
                                                   .WithPayload(command.Payload));
    }
    
    public override async Task<Result<Guid>> OnEndAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After Approval Flow End Person")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> AfterAcceptAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterAcceptAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("After Approval Flow Accept Person")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> AfterRejectAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterRejectAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("After Approval Flow Reject Person")
                                                   .WithPayload(command.Payload));
    }
}