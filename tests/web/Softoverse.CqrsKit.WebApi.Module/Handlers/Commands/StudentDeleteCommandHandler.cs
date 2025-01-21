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
    public override async Task<Result<Guid>> ValidateAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        var command = context.RequestAs<StudentDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Valid Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var command = context.RequestAs<StudentDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Before execution Student")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var command = context.RequestAs<StudentDeleteCommand>();
        await dbContext.Students.Where(x => x.Id == command.Payload).ExecuteDeleteAsync(ct);

        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("Successfully Deleted")
                                                 .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var command = context.RequestAs<StudentDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                 .WithMessage("After execution Student")
                                                 .WithPayload(command.Payload));
    }
}