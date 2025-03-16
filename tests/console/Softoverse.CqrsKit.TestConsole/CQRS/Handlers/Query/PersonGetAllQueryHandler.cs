using System.Diagnostics;

using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Attributes;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Query;

[ScopedLifetime]
[QueryAuthorize]
public class PersonGetAllQueryHandler : QueryHandler<PersonGetAllQuery, List<Person>>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<List<Person>>> OnStartAsync(PersonGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<List<Person>>.Success()
                                                         .WithMessage("Before execution Person"));
    }

    public override async Task<Result<List<Person>>> HandleAsync(PersonGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var students = _studentStore.Where(x =>
                                               (!string.IsNullOrEmpty(x.Name) && x.Name == query.Name)
                                             ||
                                               (x.Age is not null && x.Age == query.Age)
                                          ).ToList();

        return await Task.FromResult(Result<List<Person>>.Create(x => x.Payload?.Count > 0)
                                                         .WithPayload(students)
                                                         .WithSuccessMessage("Found Person data")
                                                         .WithErrorMessage("No data found"));
    }

    public override async Task<Result<List<Person>>> OnEndAsync(PersonGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<List<Person>>.Success()
                                                         .WithMessage("After execution Person"));
    }
}