using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.Builders;
using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Module.Event.Commands;
using Softoverse.CqrsKit.WebApi.Module.Event.Queries;

namespace Softoverse.CqrsKit.WebApi.Module;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IServiceProvider services, ApplicationDbContext dbContext) : ControllerBase
{
    // GET: api/Students
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] StudentGetAllQuery query, bool useCqrs = false, CancellationToken ct = default)
    {
        if (useCqrs)
        {
            var studentGetAllQuery = QueryBuilder.Initialize<StudentGetAllQuery, List<Student>>(services)
                                                 .WithQuery(query)
                                                 .Build();

            var result = await studentGetAllQuery.ExecuteAsync(ct);

            return Ok(result);
        }

        var students = await dbContext.Students.Where(x => (!string.IsNullOrEmpty(x.Name) && x.Name == query.Name)
                                                         ||
                                                           (x.Age != null && x.Age == query.Age)
                                                         ||
                                                           (!string.IsNullOrEmpty(x.Gender) && x.Gender == query.Gender))
                                      .ToListAsync(ct);

        return Ok(Result<List<Student>>.Success()
                                       .WithPayload(students)
                                       .WithMessageLogic(x => x.Payload?.Count > 0)
                                       .WithSuccessMessage("Found Student data")
                                       .WithErrorMessage("No data found"));
    }

    // GET api/Students/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var studentGetByIdQuery = QueryBuilder.Initialize<StudentGetByIdQuery, Student>(services)
                                              .WithQuery(new StudentGetByIdQuery
                                              {
                                                  Id = id
                                              })
                                              .Build();

        var result = await studentGetByIdQuery.ExecuteAsync(ct);

        return Ok(result);
    }

    // POST api/Students/5
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Student student, CancellationToken ct = default)
    {
        var studentCreateCommand = CommandBuilder.Initialize<StudentCreateCommand, Student>(services)
                                                 .WithCommand(new StudentCreateCommand(student))
                                                 .Build();

        var result = await studentCreateCommand.ExecuteAsync(ct);

        return Ok(result);
    }

    // PUT api/Students/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Student student, CancellationToken ct = default)
    {
        var studentUpdateCommand = CommandBuilder.Initialize<StudentUpdateCommand, Student>(services)
                                                 .WithCommand(new StudentUpdateCommand(id, student))
                                                 .Build();

        var result = await studentUpdateCommand.ExecuteAsync(ct);

        return Ok(result);
    }

    // DELETE api/Students/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var studentDeleteCommand = CommandBuilder.Initialize<StudentDeleteCommand, Student>(services)
                                                 .WithCommand(new StudentDeleteCommand(id))
                                                 .Build();

        var result = await studentDeleteCommand.ExecuteAsync(ct);

        return Ok(result);
    }
}