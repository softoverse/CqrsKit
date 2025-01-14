using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Query;

[ScopedLifetime]
public class StudentGetAllQueryHandler : QueryHandler<StudentGetAllQuery, List<Student>>
{
    private readonly List<Student> _studentStore = Program.StudentStore;

    public override async Task<Response<List<Student>>> OnStartAsync(StudentGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Response<List<Student>>.Success()
                                                            .WithMessage("Before execution Student"));
    }

    public override async Task<Response<List<Student>>> HandleAsync(StudentGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");

        var students = _studentStore.Where(x =>
                                               (!string.IsNullOrEmpty(x.Name) && x.Name == query.Name)
                                             ||
                                               (x.Age is not null && x.Age == query.Age)
                                          ).ToList();

        return await Task.FromResult(Response<List<Student>>.Create(students.Count > 0)
                                                            .WithPayload(students)
                                                            .WithSuccessMessage("Found Student data")
                                                            .WithErrorMessage("No data found"));
    }

    public override async Task<Response<List<Student>>> OnEndAsync(StudentGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Response<List<Student>>.Success()
                                                            .WithMessage("After execution Student"));
    }
}