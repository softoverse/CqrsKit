using CqrsKit.Abstraction.Handlers;
using CqrsKit.Attributes;
using CqrsKit.Model;
using CqrsKit.Model.Utility;
using CqrsKit.TestConsole.CQRS.Events.Query;
using CqrsKit.TestConsole.Models;

namespace CqrsKit.TestConsole.CQRS.Handlers.Query;

[ScopedLifetime]
public class StudentGetByIdQueryHandler : QueryHandler<StudentGetByIdQuery, Student>
{
    private readonly List<Student> _studentStore = Program.StudentStore;

    public override async Task<Response<Student>> OnStartAsync(StudentGetByIdQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("Before execution Student"));
    }

    public override async Task<Response<Student>> HandleAsync(StudentGetByIdQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var student = _studentStore.FirstOrDefault(x => x.Id == query.Id);

        return await Task.FromResult(Response<Student>.Create(student != null)
                                                      .WithPayload(student!)
                                                      .WithSuccessMessage("Found Student data")
                                                      .WithErrorMessage("No data found"));
    }

    public override async Task<Response<Student>> OnEndAsync(StudentGetByIdQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Response<Student>.Success()
                                                      .WithMessage("After execution Student"));
    }
}