using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Attributes;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[ScopedLifetime]
[CommandAuthorize]
public class PersonDeleteCommandHandler : CommandHandler<PersonDeleteCommand, Guid>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Guid>> ValidateAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Valid Person")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Before execution Person")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        var student = _studentStore.FirstOrDefault(x => x.Id == command.Payload);
        _studentStore.Remove(student!);

        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Successfully Deleted")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var command = context.RequestAs<PersonDeleteCommand>();
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("After execution Person")
                                                   .WithPayload(command.Payload));
    }
}