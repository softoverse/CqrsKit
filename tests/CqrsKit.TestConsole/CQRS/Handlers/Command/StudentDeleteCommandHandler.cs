using CqrsKit.Abstraction.Handlers;
using CqrsKit.Attributes;
using CqrsKit.Model;
using CqrsKit.Model.Utility;
using CqrsKit.TestConsole.CQRS.Events.Command;
using CqrsKit.TestConsole.Models;

namespace CqrsKit.TestConsole.CQRS.Handlers.Command;

[ScopedLifetime]
public class StudentDeleteCommandHandler : CommandHandler<StudentDeleteCommand, Guid>
{
    private readonly List<Student> _studentStore = Program.StudentStore;

    public override async Task<Response<Guid>> ValidateAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("Valid Student")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Response<Guid>> OnStartAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("Before execution Student")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Response<Guid>> HandleAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var student = _studentStore.FirstOrDefault(x => x.Id == command.Payload);
        _studentStore.Remove(student!);

        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("Successfully Deleted")
                                                   .WithPayload(command.Payload));
    }

    public override async Task<Response<Guid>> OnEndAsync(StudentDeleteCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Response<Guid>.Success()
                                                   .WithMessage("After execution Student")
                                                   .WithPayload(command.Payload));
    }
}