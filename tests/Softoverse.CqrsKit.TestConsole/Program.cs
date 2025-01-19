using Bogus;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Command;
using Softoverse.CqrsKit.TestConsole.CQRS.Events.Query;
using Softoverse.CqrsKit.TestConsole.Models;

using Microsoft.Extensions.DependencyInjection;

using static Softoverse.CqrsKit.TestConsole.Utility.ConsoleColor;

using Person = Softoverse.CqrsKit.TestConsole.Models.Person;

namespace Softoverse.CqrsKit.TestConsole;

public class Program
{
    public static readonly bool IsApprovalFlowEnabled = true;
    private static readonly Faker<Person> PersonFaker = InitializeFaker();
    public static List<Person> PersonStore = [];

    public static async Task Main(string[] args)
    {
        Console.WriteLine($"{Blue}Welcome to the CQRS Kit!{Normal}\n");

        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.AddCqrsKit<Program>()
                                               .AddScoped<PersonOperation>()
                                               .BuildServiceProvider();

        bool isAccept = false;

        PersonStore = SeedPerson(2);
        await StartAsync(serviceProvider, isAccept);

        Console.WriteLine($"{Blue}\nComplete with success!{Normal}");
    }

    private static async Task StartAsync(IServiceProvider serviceProvider, bool isAccept = true)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var personOperation = scope.ServiceProvider.GetRequiredService<PersonOperation>();

        Guid selectedPersonId = PersonStore.FirstOrDefault()?.Id ?? Guid.Empty;

        await personOperation.GetPerson(new PersonGetAllQuery
        {
            Name = "Abir"
        });

        await personOperation.GetPersonById(new PersonGetByIdQuery
        {
            Id = selectedPersonId
        });

        var newPerson = SeedPerson(1)[0];

        await personOperation.CreatePerson(new(newPerson));


        newPerson.Name = newPerson.Gender switch
        {
            "Male" => "Abir Mahmud",
            "Female" => "Sadiya Islam",
            _ => newPerson.Name
        };
        
        newPerson.Age = newPerson.Gender switch
        {
            "Male" => 27,
            "Female" => 22,
            _ => newPerson.Age
        };

        await personOperation.UpdatePerson(new PersonUpdateCommand(newPerson.Id, newPerson));

        await personOperation.GetPerson(new PersonGetAllQuery
        {
            Name = newPerson.Gender switch
            {
                "Male" => "Abir",
                "Female" => "Sadiya",
                _ => newPerson.Name
            },
            Age = newPerson.Gender switch
            {
                "Male" => 27,
                "Female" => 22,
                _ => newPerson.Age
            }
        });

        await personOperation.DeletePerson(new PersonDeleteCommand(newPerson.Id));
        
        await personOperation.GetPerson(new PersonGetAllQuery
        {
            Name = newPerson.Gender switch
            {
                "Male" => "Abir",
                "Female" => "Sadiya",
                _ => newPerson.Name
            },
            Age = newPerson.Gender switch
            {
                "Male" => 27,
                "Female" => 22,
                _ => newPerson.Age
            }
        });


        if (isAccept)
        {
            await personOperation.ApproveDeletePerson($"{selectedPersonId}");
        }
        else
        {
            await personOperation.RejectDeletePerson($"{selectedPersonId}");
        }

        await personOperation.GetPerson(new PersonGetAllQuery
        {
            Name = newPerson.Gender switch
            {
                "Male" => "Abir",
                "Female" => "Sadiya",
                _ => newPerson.Name
            },
            Age = newPerson.Gender switch
            {
                "Male" => 27,
                "Female" => 22,
                _ => newPerson.Age
            }
        });

        await personOperation.GetPersonById(new PersonGetByIdQuery
        {
            Id = selectedPersonId
        });
    }

    private static List<Person> SeedPerson(int count)
    {
        // PersonStore.Clear();
        return PersonFaker.Generate(count);
    }

    private static Faker<Person> InitializeFaker()
    {
        var personFaker = new Faker<Person>()
                          .RuleFor(s => s.Id, _ => Guid.NewGuid())
                          .RuleFor(s => s.Name, f => f.Person.FullName)
                          .RuleFor(s => s.Age, f => (DateTime.Now - f.Person.DateOfBirth).Days / 365)
                          .RuleFor(s => s.Gender, f => f.Person.Gender.ToString());
        return personFaker;
    }
}