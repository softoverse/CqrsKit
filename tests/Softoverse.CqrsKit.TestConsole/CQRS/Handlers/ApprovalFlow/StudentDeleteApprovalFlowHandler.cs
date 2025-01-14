using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.ApprovalFlow;

[TransientLifetime]
public class StudentDeleteApprovalFlowHandler : ApprovalFlowHandler<StudentDeleteCommand, Guid>
{
    public override async Task<Response<Guid>> OnStartAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        context.Items.Add("Test", "TEST");
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("Before Approval Flow Start Student")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Response<Guid>> OnEndAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("After Approval Flow End Student")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Response<Guid>> AfterAcceptAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterAcceptAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("After Approval Flow Accept Student")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Response<Guid>> AfterRejectAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterRejectAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("After Approval Flow Reject Student")
                                                   .WithPayload(command.Payload));
    }
}