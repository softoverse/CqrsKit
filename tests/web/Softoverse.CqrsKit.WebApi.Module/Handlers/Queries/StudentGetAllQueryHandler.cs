using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.Abstraction.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.Model.Utility;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Module.Event.Queries;

namespace Softoverse.CqrsKit.WebApi.Module.Handlers.Queries;

[ScopedLifetime]
public class StudentGetAllQueryHandler(ApplicationDbContext dbContext) : QueryHandler<StudentGetAllQuery, List<Student>>
{
    public override async Task<Result<List<Student>>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<List<Student>>.Success()
                                                          .WithMessage("Before execution Student"));
    }

    public override async Task<Result<List<Student>>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var query = context.RequestAs<StudentGetAllQuery>();

        var students = await dbContext.Students.Where(x => (!string.IsNullOrEmpty(x.Name) && x.Name == query.Name) 
                                                           ||
                                                           (x.Age != null && x.Age == query.Age)
                                                           ||
                                                           (!string.IsNullOrEmpty(x.Gender) && x.Gender == query.Gender))
                                                .ToListAsync(ct);

        return Result<List<Student>>.Success()
                                    .WithPayload(students)
                                    .WithMessageLogic(students.Count > 0)
                                    .WithSuccessMessage("Found Student data")
                                    .WithErrorMessage("No data found");
    }

    public override async Task<Result<List<Student>>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<List<Student>>.Success()
                                                          .WithMessage("After execution Student"));
    }
}