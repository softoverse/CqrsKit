using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.WebApi.CQRS.Events.Students.Commands;

namespace Softoverse.CqrsKit.WebApi.CQRS.Handlers.Students.ApprovalFlows;

[TransientLifetime]
public class StudentDeleteApprovalFlowHandler : ApprovalFlowHandler<StudentDeleteCommand, Guid>
{
    public override async Task<Result<Guid>> OnStartAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Before Approval Flow Start Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnEndAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After Approval Flow End Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> AfterAcceptAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterAcceptAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After Approval Flow Accept Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> AfterRejectAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.AfterRejectAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After Approval Flow Reject Student")
                                                 .WithPayload(command.Payload));
    }
}