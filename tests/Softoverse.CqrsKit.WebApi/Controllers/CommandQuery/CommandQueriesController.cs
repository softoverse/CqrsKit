using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.ViewModels;

namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandQueriesController(ApplicationDbContext dbContext) : ControllerBase
    {
        // GET: api/CommandQueries
        [HttpGet]
        public async Task<IActionResult> GetCommandQueries([FromQuery] RequestType type = RequestType.Both)
        {
            IQueryable<Models.CQRS.CommandQuery> query = (type) switch
            {
                RequestType.Command => dbContext.CommandQueries.Where(x => x.IsCommand),
                RequestType.Query => dbContext.CommandQueries.Where(x => !x.IsCommand),
                _ => dbContext.CommandQueries
            };

            var commandQueries = await query.ToListAsync();
            return Ok(commandQueries);
        }

        // GET: api/CommandQueries/Commands
        [HttpGet("Commands")]
        public async Task<IActionResult> GetCommands()
        {
            return await GetCommandQueries(RequestType.Command);
            var commands = await dbContext.CommandQueries.Where(x => x.IsCommand).ToListAsync();
            return Ok(commands);
        }

        // GET: api/CommandQueries/Queries
        [HttpGet("Queries")]
        public async Task<IActionResult> GetQueries()
        {
            return await GetCommandQueries(RequestType.Query);
            var queries = await dbContext.CommandQueries.Where(x => !x.IsCommand).ToListAsync();
            return Ok(queries);
        }

        // GET: api/CommandQueries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommandQuery(long id)
        {
            var command = await dbContext.CommandQueries.Include(x => x.CommandApprovalFlowConfiguration).FirstOrDefaultAsync(x => x.Id == id);

            if (command == null) return NotFound();
            return Ok(command);
        }
    }
}