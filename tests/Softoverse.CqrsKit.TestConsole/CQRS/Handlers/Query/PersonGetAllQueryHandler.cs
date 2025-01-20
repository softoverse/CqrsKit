using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Query;

[ScopedLifetime]
public class PersonGetAllQueryHandler : QueryHandler<PersonGetAllQuery, List<Person>>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<List<Person>>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var query = context.RequestAs<PersonGetAllQuery>();
        return await Task.FromResult(Result<List<Person>>.Success()
                                                            .WithMessage("Before execution Person"));
    }

    public override async Task<Result<List<Person>>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var query = context.RequestAs<PersonGetAllQuery>();
        var students = _studentStore.Where(x =>
                                               (!string.IsNullOrEmpty(x.Name) && x.Name == query.Name)
                                             ||
                                               (x.Age is not null && x.Age == query.Age)
                                          ).ToList();

        return await Task.FromResult(Result<List<Person>>.Create(students.Count > 0)
                                                            .WithPayload(students)
                                                            .WithSuccessMessage("Found Person data")
                                                            .WithErrorMessage("No data found"));
    }

    public override async Task<Result<List<Person>>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var query = context.RequestAs<PersonGetAllQuery>();
        return await Task.FromResult(Result<List<Person>>.Success()
                                                            .WithMessage("After execution Person"));
    }
}