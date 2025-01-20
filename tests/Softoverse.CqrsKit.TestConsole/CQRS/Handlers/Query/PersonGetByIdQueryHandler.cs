using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Query;

[ScopedLifetime]
public class PersonGetByIdQueryHandler : QueryHandler<PersonGetByIdQuery, Person>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Person>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        var query = context.RequestAs<PersonGetByIdQuery>();
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("Before execution Person"));
    }

    public override async Task<Result<Person>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var query = context.RequestAs<PersonGetByIdQuery>();
        var student = _studentStore.FirstOrDefault(x => x.Id == query.Id);

        return await Task.FromResult(Result<Person>.Create(student != null)
                                                      .WithPayload(student!)
                                                      .WithSuccessMessage("Found Person data")
                                                      .WithErrorMessage("No data found"));
    }

    public override async Task<Result<Person>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        var query = context.RequestAs<PersonGetByIdQuery>();
        return await Task.FromResult(Result<Person>.Success()
                                                      .WithMessage("After execution Person"));
    }
}