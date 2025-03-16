using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.Abstractions.Handlers;
using Softoverse.CqrsKit.Attributes;
using Softoverse.CqrsKit.Models;
using Softoverse.CqrsKit.Models.Utility;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Models.ClassModule;
using Softoverse.CqrsKit.WebApi.Module.Attributes;
using Softoverse.CqrsKit.WebApi.Module.Event.Queries;

namespace Softoverse.CqrsKit.WebApi.Module.Handlers.Queries;

[ScopedLifetime]
[QueryAuthorize]
public class StudentGetAllQueryHandler(ApplicationDbContext dbContext) : QueryHandler<StudentGetAllQuery, List<Student>>
{
    public override async Task<Result<List<Student>>> OnStartAsync(StudentGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        return await Task.FromResult(Result<List<Student>>.Success()
                                                          .WithMessage("Before execution Student"));
    }

    public override async Task<Result<List<Student>>> HandleAsync(StudentGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        var students = await dbContext.Students.Where(x => (!string.IsNullOrEmpty(x.Name) && x.Name == query.Name)
                                                         ||
                                                           (x.Age != null && x.Age == query.Age)
                                                         ||
                                                           (!string.IsNullOrEmpty(x.Gender) && x.Gender == query.Gender))
                                      .ToListAsync(ct);

        return Result<List<Student>>.Success()
                                    .WithPayload(students)
                                    .WithMessageLogic(x => x.Payload?.Count > 0)
                                    .WithSuccessMessage("Found Student data")
                                    .WithErrorMessage("No data found");
    }

    public override async Task<Result<List<Student>>> OnEndAsync(StudentGetAllQuery query, CqrsContext context, CancellationToken ct = default)
    {
        return await Task.FromResult(Result<List<Student>>.Success()
                                                          .WithMessage("After execution Student"));
    }
}