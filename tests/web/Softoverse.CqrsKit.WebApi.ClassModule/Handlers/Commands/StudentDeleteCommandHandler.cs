using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Module.Event.Commands;

namespace Softoverse.CqrsKit.WebApi.Module.Handlers.Commands;

[ScopedLifetime]
public class StudentDeleteCommandHandler(ApplicationDbContext dbContext) : CommandHandler<StudentDeleteCommand, Guid>
{
    public override async Task<Result<Guid>> ValidateAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Valid Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnStartAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Before execution Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> HandleAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        await dbContext.Students.Where(x => x.Id == command.Payload).ExecuteDeleteAsync(ct);

        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Successfully Deleted")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnEndAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After execution Student")
                                                 .WithPayload(command.Payload));
    }
}