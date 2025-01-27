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
public class StudentGetByIdQueryHandler(ApplicationDbContext dbContext) : QueryHandler<StudentGetByIdQuery, Student>
{

    public override async Task<Result<Student>> OnStartAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnStartAsync)}");
        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("Before execution Student"));
    }

    public override async Task<Result<Student>> HandleAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.HandleAsync)}");
        var query = context.RequestAs<StudentGetByIdQuery>();
        var student = await dbContext.Students.FirstOrDefaultAsync(x => x.Id == query.Id, ct);



        return Result<Student>.Success()
                              .WithPayload(student!)
                              .WithMessageLogic(x => x.Payload != null)
                              .WithSuccessMessage("Found Student data")
                              .WithErrorMessage("No data found");
    }

    public override async Task<Result<Student>> OnEndAsync(CqrsContext context, CancellationToken ct = default)
    {
        Console.WriteLine($"Method Call: {this.GetType().Name}.{nameof (this.OnEndAsync)}");
        return await Task.FromResult(Result<Student>.Success()
                                                    .WithMessage("After execution Student"));
    }
}