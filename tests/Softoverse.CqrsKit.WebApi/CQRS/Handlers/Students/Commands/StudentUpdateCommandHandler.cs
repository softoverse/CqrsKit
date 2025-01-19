using FluentValidation;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.WebApi.CQRS.Events.Students.Commands;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.CQRS.Handlers.Students.Commands;

[ScopedLifetime]
public class StudentUpdateCommandHandler(ApplicationDbContext dbContext, IValidator<Student> validator) : CommandHandler<StudentUpdateCommand, Student>
{
    public override async Task<Result<Student>> ValidateAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.ValidateAsync)}");
        Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

        if (command.Id != command.Payload.Id)
            errors.AddError("Id", ["'Id' is invalid"]);

        var validationResult = await validator.ValidateAsync(command.Payload, ct);

        if (!validationResult.IsValid)
        {
            errors.AddErrors(validationResult.ToDictionary());
        }

        if (errors.Count > 0)
            return await Task.FromResult(Result<Student>.Error()
                                                        .WithMessage("Validation failed")
                                                        .WithPayload(command.Payload)
                                                        .WithErrors(errors));

        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("Valid Person")
                                                    .WithPayload(command.Payload));
    }

    public override async Task<Result<Student>> OnStartAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");

        command.Payload.AgeCategory = command.Payload.Age switch
        {
            < 2 => AgeCategory.Infant,
            >= 2 and <= 12 => AgeCategory.Child,
            >= 13 and < 18 => AgeCategory.Teenager,
            >= 18 => AgeCategory.Adult,
            _ => throw new ArgumentOutOfRangeException(nameof (command.Payload.Age), "Invalid age")
        };

        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("Before execution Person")
                                                    .WithPayload(command.Payload));
    }

    public override async Task<Result<Student>> HandleAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");

        Student currentStudent = command.Payload;
        dbContext.Students.Update(currentStudent);
        await dbContext.SaveChangesAsync(ct);

        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("Successfully Updated")
                                                    .WithPayload(currentStudent));
    }

    public override async Task<Result<Student>> OnEndAsync(StudentUpdateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("After execution Person")
                                                    .WithPayload(command.Payload));
    }
}