using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(ApplicationDbContext dbContext)
    : ControllerBase
{
    // GET: api/Students
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = Result<IList<Student>>.Success()
                                           .WithPayload(await dbContext.Students.ToListAsync())
                                           .WithMessage("Students retrieved successfully");

        return Ok(result);
    }

    // GET api/Students/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = Result<Student?>.Success()
                                     .WithPayload(await dbContext.Students.FindAsync(id))
                                     .WithMessage("Student retrieved successfully");

        return Ok(result);
    }


    // POST api/Students/5
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Student student)
    {
        var addResult = await dbContext.Students.AddAsync(student);
        await dbContext.SaveChangesAsync();

        var result = Result<Student>.Success()
                                    .WithPayload(student)
                                    .WithMessage("Student updated successfully");

        return Ok(result);
    }

    // PUT api/Students/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Student student)
    {
        var updateResult = dbContext.Students.Update(student);
        await dbContext.SaveChangesAsync();

        var result = Result<Student>.Success()
                                    .WithPayload(student)
                                    .WithMessage("Student updated successfully");

        return Ok(result);
    }

    // DELETE api/Students/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await dbContext.Students.Where(x => x.Id == id).ExecuteDeleteAsync();
        await dbContext.SaveChangesAsync();

        var result = Result<Guid>.Success()
                                 .WithPayload(id)
                                 .WithMessage("Student deleted successfully");

        return Ok(result);
    }
}