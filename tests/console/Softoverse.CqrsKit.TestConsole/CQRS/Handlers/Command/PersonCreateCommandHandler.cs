using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Attributes;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[ScopedLifetime]
[CommandAuthorize]
[CommandValidate]
public class PersonCreateCommandHandler : CommandHandler<PersonCreateCommand, Person>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Person>> ValidateAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        var command = context.RequestAs<PersonCreateCommand>();
        if (_studentStore.Any(x => x.Name == command.Payload.Name))
        {
            string errorMessage = "Person name already exists";

            IDictionary<string, string[]> errors = new Dictionary<string, string[]>().AddError("Name", errorMessage);

            return await Task.FromResult(Result<Person>.Error()
                                                               .WithMessage(errorMessage)
                                                               .WithPayload(command.Payload)
                                                               .WithErrors(errors));
        }

        return await Task.FromResult(Result<Person>.Success()
                                                           .WithMessage("Valid Person")
                                                           .WithPayload(command.Payload));
    }

    public override async Task<Result<Person>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var command = context.RequestAs<PersonCreateCommand>();
        return await Task.FromResult(Result<Person>.Success()
                                                           .WithMessage("Before execution Person")
                                                           .WithPayload(command.Payload));
    }

    public override async Task<Result<Person>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var command = context.RequestAs<PersonCreateCommand>();
        Person person = command.Payload;
        _studentStore.Add(person);

        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Successfully Created")
                                                      .WithPayload(person));
    }

    public override async Task<Result<Person>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var command = context.RequestAs<PersonCreateCommand>();
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("After execution Person")
                                                      .WithPayload(command.Payload));
    }
}