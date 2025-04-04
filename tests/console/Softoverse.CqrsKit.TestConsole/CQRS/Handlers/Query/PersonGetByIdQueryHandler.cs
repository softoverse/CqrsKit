﻿using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

namespace Softoverse.CqrsKit.TestConsole.CQRS.Handlers.Query;

[ScopedLifetime]
public class PersonGetByIdQueryHandler : QueryHandler<PersonGetByIdQuery, Person>
{
    private readonly List<Person> _studentStore = Program.PersonStore;

    public override async Task<Result<Person>> OnStartAsync(PersonGetByIdQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<Person>.Success()
                                                   .WithMessage("Before execution Person"));
    }

    public override async Task<Result<Person>> HandleAsync(PersonGetByIdQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var student = _studentStore.FirstOrDefault(x => x.Id == query.Id);

        return await Task.FromResult(Result<Person>.Create(x => x.Payload != null)
                                                   .WithPayload(student!)
                                                   .WithSuccessMessage("Found Person data")
                                                   .WithErrorMessage("No data found"));
    }

    public override async Task<Result<Person>> OnEndAsync(PersonGetByIdQuery query, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Person>.Success()
                                                   .WithMessage("After execution Person"));
    }
}