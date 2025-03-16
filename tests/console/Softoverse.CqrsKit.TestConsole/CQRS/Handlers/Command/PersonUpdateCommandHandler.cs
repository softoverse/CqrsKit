using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Extensions;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[SingletonLifetime]
public class PersonUpdateCommandHandler : CommandHandler<PersonUpdateCommand, Person>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Person>> ValidateAsync(PersonUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

        if (command.Id != command.Payload.Id)
            errors.AddError("Id", ["'Id' is invalid"]);

        // if (_studentStore.Any(x => x.Name == command.Payload.Name))
        //     errors.AddError("Name", ["Person name already exists"]);

        if (errors.Count > 0)
            return await Task.FromResult(Result<Person>.Error()
                                                          .WithMessage("Validation Error")
                                                          .WithPayload(command.Payload)
                                                          .WithErrors(errors));

        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Valid Person")
                                                      .WithPayload(command.Payload));
    }

    public override async Task<Result<Person>> OnStartAsync(PersonUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Before execution Person")
                                                      .WithPayload(command.Payload));
    }

    public override async Task<Result<Person>> HandleAsync(PersonUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var currentStudent = _studentStore.FirstOrDefault(x => x.Id == command.Id);
        _studentStore.Remove(currentStudent!);
        currentStudent = command.Payload;
        _studentStore.Add(currentStudent);

        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Successfully Updated")
                                                      .WithPayload(currentStudent));
    }

    public override async Task<Result<Person>> OnEndAsync(PersonUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("After execution Person")
                                                      .WithPayload(command.Payload));
    }
}