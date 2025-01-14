using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[SingletonLifetime]
public class StudentUpdateCommandHandler : CommandHandler<StudentUpdateCommand, Student>
{
    private readonly List<Student> _studentStore = Program.StudentStore;

    public override async Task<Response<Student>> ValidateAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

        if (command.Id != command.Payload.Id)
            errors.AddError("Id", ["'Id' is invalid"]);

        // if (_studentStore.Any(x => x.Name == command.Payload.Name))
        //     errors.AddError("Name", ["Student name already exists"]);

        if (errors.Count > 0)
            return await Task.FromResult(Response<Student>.Error()
                                                          .WithMessage("Validation Error")
                                                          .WithPayload(command.Payload)
                                                          .WithErrors(errors));

        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("Valid Student")
                                                      .WithPayload(command.Payload));
    }

    public override async Task<Response<Student>> OnStartAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("Before execution Student")
                                                      .WithPayload(command.Payload));
    }

    public override async Task<Response<Student>> HandleAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var currentStudent = _studentStore.FirstOrDefault(x => x.Id == command.Id);
        _studentStore.Remove(currentStudent!);
        currentStudent = command.Payload;
        _studentStore.Add(currentStudent);

        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("Successfully Updated")
                                                      .WithPayload(currentStudent));
    }

    public override async Task<Response<Student>> OnEndAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("After execution Student")
                                                      .WithPayload(command.Payload));
    }
}