using FluentValidation;

using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.ModelValidations;

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
    {
        // Attributes in the model are for EF Core and DbContext validation. not for FluentValidation

        // RuleFor methods are for FluentValidation
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Gender).NotEmpty();
        RuleFor(x => x.Age).NotNull().InclusiveBetween(0, 200);
        RuleFor(x => x.AgeCategory).Null();
    }
}