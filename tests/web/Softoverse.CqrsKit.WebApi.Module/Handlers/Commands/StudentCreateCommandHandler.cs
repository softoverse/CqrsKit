using FluentValidation;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Extensions;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Module.Attributes;
using Softoverse.CqrsKit.WebApi.Module.Event.Commands;

namespace Softoverse.CqrsKit.WebApi.Module.Handlers.Commands;

[ScopedLifetime]
[CommandAuthorize]
public class StudentCreateCommandHandler(ApplicationDbContext dbContext, IValidator<Student> validator) : CommandHandler<StudentCreateCommand, Student>
{
    public override async Task<Result<Student>> ValidateAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(command.Payload, ct);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.ToDictionary();

            return Result<Student>.Error()
                                  .WithPayload(command.Payload)
                                  .WithMessage("Validation failed")
                                  .WithErrors(validationErrors);
        }

        if (!dbContext.Students.Any(x => x.Name == command.Payload.Name))
        {
            return await Task.FromResult(Result<Student>.Success()
                                                        .WithMessage("Valid Student")
                                                        .WithPayload(command.Payload));
        }

        string errorMessage = "Student name already exists";

        IDictionary<string, string[]> errors = new Dictionary<string, string[]>().AddError("Name", errorMessage);

        return await Task.FromResult(Result<Student>.Error()
                                                    .WithMessage(errorMessage)
                                                    .WithPayload(command.Payload)
                                                    .WithErrors(errors));
    }

    public override async Task<Result<Student>> OnStartAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        command.Payload.AgeCategory = command.Payload.Age switch
        {
            < 2 => AgeCategory.Infant,
            >= 2 and <= 12 => AgeCategory.Child,
            >= 13 and < 18 => AgeCategory.Teenager,
            >= 18 => AgeCategory.Adult,
            _ => throw new ArgumentOutOfRangeException(nameof (command.Payload.Age), "Invalid age")
        };

        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("Before execution Student")
                                                    .WithPayload(command.Payload));
    }

    public override async Task<Result<Student>> HandleAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        Student student = command.Payload;
        dbContext.Students.Add(student);
        // await dbContext.SaveChangesAsync(ct);

        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("Successfully Created")
                                                    .WithPayload(student));
    }

    public override async Task<Result<Student>> OnEndAsync(StudentCreateCommand command, CqrsContext context, CancellationToken ct = default)
    {
        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("After execution Student")
                                                    .WithPayload(command.Payload));
    }
}