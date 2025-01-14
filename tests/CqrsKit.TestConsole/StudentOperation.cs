using System.Diagnostics;
using System.Text.Json;

using CqrsKit.Builders;
using CqrsKit.Model;
using CqrsKit.Model.Entity;
using CqrsKit.TestConsole.CQRS.Events.Command;
using CqrsKit.TestConsole.CQRS.Events.Query;
using CqrsKit.TestConsole.Models;

using static CqrsKit.TestConsole.Utility.ConsoleHelper;
using static CqrsKit.TestConsole.Utility.ConsoleColor;


namespace CqrsKit.TestConsole;

public class StudentOperation(IServiceProvider services)
{
    public async Task GetStudents(StudentGetAllQuery request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(GetStudents), startsWith: Green, endsWith: Normal);
        
        var sw = Stopwatch.StartNew();

        var query = CqrsBuilder.Query<StudentGetAllQuery, List<Student>>(services)
                               .WithQuery(request)
                               .Build();

        var students = await query.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        // if (!students.IsSuccessful) PrintErrors(students.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(students);
        EndBlock();
    }

    public async Task GetStudentById(StudentGetByIdQuery request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(GetStudentById), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var query = QueryBuilder.Initialize<StudentGetByIdQuery, Student>(services)
                                .WithQuery(request)
                                .Build();

        var student = await query.ExecuteDefaultAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        // if (!student.IsSuccessful) PrintErrors(student.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(student);
        EndBlock();
    }

    public async Task CreateStudent(StudentCreateCommand request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(CreateStudent), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var command = CommandBuilder.Initialize<StudentCreateCommand, Student>(services)
                                    .WithCommand(request)
                                    .Build();

        var studentCreateResponse = await command.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (!studentCreateResponse.IsSuccessful) PrintErrors(studentCreateResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request.Payload);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(studentCreateResponse);
        EndBlock();
    }

    public async Task UpdateStudent(StudentUpdateCommand request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(UpdateStudent), startsWith: Green, endsWith: Normal);
        var student = request.Payload;

        var sw = Stopwatch.StartNew();

        var command = CommandBuilder.Initialize<StudentUpdateCommand, Student>(services)
                                    .WithCommand(request)
                                    .Build();

        var studentUpdateResponse = await command.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (!studentUpdateResponse.IsSuccessful) PrintErrors(studentUpdateResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request.Payload);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(studentUpdateResponse);
        EndBlock();
    }

    public async Task DeleteStudent(StudentDeleteCommand request, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(DeleteStudent), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var command = CqrsBuilder.Command<StudentDeleteCommand, Guid>(services)
                                 .WithCommand(request)
                                 .WithApprovalFlow()
                                 .Build();

        var studentDeleteResponse = await command.ExecuteAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (!studentDeleteResponse.IsSuccessful) PrintErrors(studentDeleteResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(request.Payload);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(studentDeleteResponse);
        EndBlock();
    }

    public async Task ApproveDeleteStudent(string approvalFlowId, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(ApproveDeleteStudent), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var approvalFlow = ApprovalFlowBuilder.Initialize<BaseApprovalFlowPendingTask>(services)
                                              .WithId(approvalFlowId)
                                              .Build();

        var approveDeleteStudentResponse = await approvalFlow.AcceptAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (!approveDeleteStudentResponse.IsSuccessful) PrintErrors(approveDeleteStudentResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(approvalFlowId);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(approveDeleteStudentResponse);
        EndBlock();
    }

    public async Task RejectDeleteStudent(string approvalFlowId, CancellationToken ct = default)
    {
        StartBlock();
        PrintBlock(nameof(RejectDeleteStudent), startsWith: Green, endsWith: Normal);

        var sw = Stopwatch.StartNew();

        var approvalFlow = CqrsBuilder.ApprovalFlow<BaseApprovalFlowPendingTask>(services)
                                      .WithId(approvalFlowId)
                                      .Build();

        var approveDeleteStudentResponse = await approvalFlow.RejectAsync(ct);

        sw.Stop();
        PrintElapsedTime(sw);

        if (!approveDeleteStudentResponse.IsSuccessful) PrintErrors(approveDeleteStudentResponse.Errors);
        PrintBlock($"\nRequest:", startsWith: Green, endsWith: Normal);
        PrintPayload(approvalFlowId);
        PrintBlock($"\nResponse:", startsWith: Green, endsWith: Normal);
        PrintPayload(approveDeleteStudentResponse);
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