using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Command;

[ScopedLifetime]
public class StudentCreateCommandHandler : CommandHandler<StudentCreateCommand, Student>
{
    private readonly List<Student> _studentStore = Program.StudentStore;

    public override async Task<Response<Student>> ValidateAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");

        if (_studentStore.Any(x => x.Name == command.Payload.Name))
        {
            string errorMessage = "Student name already exists";

            IDictionary<string, string[]> errors = new Dictionary<string, string[]>().AddError("Name", errorMessage);

            return await Task.FromResult(Response<Student>.Error()
                                                               .WithMessage(errorMessage)
                                                               .WithPayload(command.Payload)
                                                               .WithErrors(errors));
        }

        return await Task.FromResult(Response<Student>.Success()
                                                           .WithMessage("Valid Student")
                                                           .WithPayload(command.Payload));
    }

    public override async Task<Response<Student>> OnStartAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Response<Student>.Success()
                                                           .WithMessage("Before execution Student")
                                                           .WithPayload(command.Payload));
    }

    public override async Task<Response<Student>> HandleAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        Student student = command.Payload;
        _studentStore.Add(student);

        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("Successfully Created")
                                                      .WithPayload(student));
    }

    public override async Task<Response<Student>> OnEndAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("After execution Student")
                                                      .WithPayload(command.Payload));
    }
}