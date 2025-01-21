using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[SingletonLifetime]
public class PersonUpdateCommandHandler : CommandHandler<PersonUpdateCommand, Person>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Person>> ValidateAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        var command = context.RequestAs<PersonUpdateCommand>();
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

    public override async Task<Result<Person>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var command = context.RequestAs<PersonUpdateCommand>();
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Before execution Person")
                                                      .WithPayload(command.Payload));
    }

    public override async Task<Result<Person>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var command = context.RequestAs<PersonUpdateCommand>();
        var currentStudent = _studentStore.FirstOrDefault(x => x.Id == command.Id);
        _studentStore.Remove(currentStudent!);
        currentStudent = command.Payload;
        _studentStore.Add(currentStudent);

        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Successfully Updated")
                                                      .WithPayload(currentStudent));
    }

    public override async Task<Result<Person>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var command = context.RequestAs<PersonUpdateCommand>();
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("After execution Person")
                                                      .WithPayload(command.Payload));
    }
}