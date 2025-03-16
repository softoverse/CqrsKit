using System.Diagnostics;
using System.Text.Json;

using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Models.Entity;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

using static Softoverse.CqrsKit.TestConsole.Utility.ConsoleHelper;
using static Softoverse.CqrsKit.TestConsole.Utility.ConsoleColor;


namespace Softoverse.CqrsKit.TestConsole;

public class PersonOperation(IServiceProvider services)
{
    public async Task GetPerson(PersonGetAllQuery request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (GetPerson), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var query = CqrsBuilder.Query<PersonGetAllQuery, List<Person>>(services)
                               .WithQuery(request)
                               .Build();

        var people = await query.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        // if (students.IsFailure) PrintErrors(students.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(people);
        EndBlock();
    }

    public async Task GetPersonById(PersonGetByIdQuery request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (GetPersonById), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var query = CqrsBuilder.Query<PersonGetByIdQuery, Person>(services)
                               .WithQuery(request)
                               .Build();

        var person = await query.ExecuteDefaultAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        // if (student.IsFailure) PrintErrors(student.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(person);
        EndBlock();
    }

    public async Task CreatePerson(PersonCreateCommand request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (CreatePerson), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var command = CqrsBuilder.Command<PersonCreateCommand, Person>(services)
                                 .WithCommand(request)
                                 .Build();

        var personCreateResponse = await command.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (personCreateResponse.IsFailure) PrintErrors(personCreateResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request.Payload);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(personCreateResponse);
        EndBlock();
    }

    public async Task UpdatePerson(PersonUpdateCommand request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (UpdatePerson), startsWith: Green, endsWith: Normal);
        var student = request.Payload;

        var sw = Stopwatch.StartNew();

        var command = CqrsBuilder.Command<PersonUpdateCommand, Person>(services)
                                 .WithCommand(request)
                                 .Build();

        var personUpdateResponse = await command.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (personUpdateResponse.IsFailure) PrintErrors(personUpdateResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request.Payload);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(personUpdateResponse);
        EndBlock();
    }

    public async Task DeletePerson(PersonDeleteCommand request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (DeletePerson), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var command = CqrsBuilder.Command<PersonDeleteCommand, Guid>(services)
                                 .WithCommand(request)
                                 .WithApprovalFlow()
                                 .Build();

        var personDeleteResponse = await command.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (personDeleteResponse.IsFailure) PrintErrors(personDeleteResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request.Payload);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(personDeleteResponse);
        EndBlock();
    }

    public async Task ApproveDeletePerson(string approvalFlowId, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (ApproveDeletePerson), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var approvalFlow = CqrsBuilder.ApprovalFlow<BaseApprovalFlowPendingTask>(services)
                                      .WithId(approvalFlowId)
                                      .Build();

        var approveDeletePersonResponse = await approvalFlow.AcceptAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (approveDeletePersonResponse.IsFailure) PrintErrors(approveDeletePersonResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(approvalFlowId);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(approveDeletePersonResponse);
        EndBlock();
    }

    public async Task RejectDeletePerson(string approvalFlowId, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof (RejectDeletePerson), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var approvalFlow = CqrsBuilder.ApprovalFlow<BaseApprovalFlowPendingTask>(services)
                                      .WithId(approvalFlowId)
                                      .Build();

        var approveDeletePersonResponse = await approvalFlow.RejectAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (approveDeletePersonResponse.IsFailure) PrintErrors(approveDeletePersonResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(approvalFlowId);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(approveDeletePersonResponse);
        EndBlock();
    }



    private static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    private static void PrintPayload<T>(T payload)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(Serialize(payload));
        Console.ResetColor();
    }

    private static void PrintElapsedTime(Stopwatch stopwatch)
    {
        // PrintBlock($"{stopwatch.Elapsed.TotalNanoseconds} ns", startsWith: Green, endsWith: Normal);
        PrintBlock($"{stopwatch.Elapsed.TotalMicroseconds} μs", startsWith: Green, endsWith: Normal);
        // PrintBlock($"{stopwatch.Elapsed.TotalMilliseconds} ms", startsWith: Green, endsWith: Normal);
        // PrintBlock($"{stopwatch.ElapsedTicks} ticks", startsWith: Green, endsWith: Normal);
    }
}