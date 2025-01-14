using Bogus;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

using Microsoft.Extensions.DependencyInjection;

using static Softoverse.CqrsKit.TestConsole.Utility.ConsoleColor;

namespace Softoverse.CqrsKit.TestConsole;

public class Program
{
    public static readonly bool IsApprovalFlowEnabled = true;
    private static readonly Faker<Student> StudentFaker = InitializeFaker();
    public static List<Student> StudentStore = [];

    public static async Task Main(string[] args)
    {
        Console.WriteLine($"{Blue}Welcome to the CQRS Kit!{Normal}\n");

        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.AddCqrsKit<Program>()
                                               .AddScoped<StudentOperation>()
                                               .BuildServiceProvider();

        bool isAccept = false;

        StudentStore = SeedStudents(2);
        await StartAsync(serviceProvider, isAccept);

        Console.WriteLine($"{Blue}\nComplete with success!{Normal}");
    }

    private static async Task StartAsync(IServiceProvider serviceProvider, bool isAccept = true)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var studentOperation = scope.ServiceProvider.GetRequiredService<StudentOperation>();

        Guid selectedStudentId = StudentStore.FirstOrDefault()?.Id ?? Guid.Empty;

        await studentOperation.GetStudents(new StudentGetAllQuery
        {
            Name = "Abir"
        });

        await studentOperation.GetStudentById(new StudentGetByIdQuery
        {
            Id = selectedStudentId
        });

        var newStudent = SeedStudents(1)[0];

        await studentOperation.CreateStudent(new(newStudent));


        newStudent.Name = newStudent.Gender switch
        {
            "Male" => "Abir Mahmud",
            "Female" => "Sadiya Islam",
            _ => newStudent.Name
        };
        
        newStudent.Age = newStudent.Gender switch
        {
            "Male" => 27,
            "Female" => 22,
            _ => newStudent.Age
        };

        await studentOperation.UpdateStudent(new StudentUpdateCommand(newStudent.Id, newStudent));

        await studentOperation.GetStudents(new StudentGetAllQuery
        {
            Name = newStudent.Gender switch
            {
                "Male" => "Abir",
                "Female" => "Sadiya",
                _ => newStudent.Name
            },
            Age = newStudent.Gender switch
            {
                "Male" => 27,
                "Female" => 22,
                _ => newStudent.Age
            }
        });

        await studentOperation.DeleteStudent(new StudentDeleteCommand(newStudent.Id));
        
        await studentOperation.GetStudents(new StudentGetAllQuery
        {
            Name = newStudent.Gender switch
            {
                "Male" => "Abir",
                "Female" => "Sadiya",
                _ => newStudent.Name
            },
            Age = newStudent.Gender switch
            {
                "Male" => 27,
                "Female" => 22,
                _ => newStudent.Age
            }
        });


        if (isAccept)
        {
            await studentOperation.ApproveDeleteStudent($"{selectedStudentId}");
        }
        else
        {
            await studentOperation.RejectDeleteStudent($"{selectedStudentId}");
        }

        await studentOperation.GetStudents(new StudentGetAllQuery
        {
            Name = newStudent.Gender switch
            {
                "Male" => "Abir",
                "Female" => "Sadiya",
                _ => newStudent.Name
            },
            Age = newStudent.Gender switch
            {
                "Male" => 27,
                "Female" => 22,
                _ => newStudent.Age
            }
        });

        await studentOperation.GetStudentById(new StudentGetByIdQuery
        {
            Id = selectedStudentId
        });
    }

    private static List<Student> SeedStudents(int count)
    {
        // StudentStore.Clear();
        return StudentFaker.Generate(count);
    }

    private static Faker<Student> InitializeFaker()
    {
        var studentFaker = new Faker<Student>()
                           .RuleFor(s => s.Id, _ => Guid.NewGuid())
                           .RuleFor(s => s.Name, f => f.Person.FullName)
                           .RuleFor(s => s.Age, f => (DateTime.Now - f.Person.DateOfBirth).Days / 365)
                           .RuleFor(s => s.Gender, f => f.Person.Gender.ToString());
        return studentFaker;
    }
}