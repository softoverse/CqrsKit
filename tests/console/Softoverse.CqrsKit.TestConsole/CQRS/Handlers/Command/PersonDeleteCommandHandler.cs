using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Attributes;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[ScopedLifetime]
[CommandValidate]
public class PersonDeleteCommandHandler : CommandHandler<PersonDeleteCommand, Guid>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Guid>> ValidateAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Valid Person")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnStartAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Before execution Person")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> HandleAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var student = _studentStore.FirstOrDefault(x => x.Id == command.Payload);
        _studentStore.Remove(student!);

        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("Successfully Deleted")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Result<Guid>> OnEndAsync(PersonDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Guid>.Success()
                                                   .WithMessage("After execution Person")
                                                   .WithPayload(command.Payload));
    }
}