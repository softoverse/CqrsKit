using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;

namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandQueriesController(ApplicationDbContext context) : ControllerBase
    {
        // GET: api/CommandQueries
        [HttpGet]
        public async Task<IActionResult> GetCommandQueries()
        {
            if (context.CommandQueries == null)
            {
                return NotFound();
            }
            return Ok(await context.CommandQueries.ToListAsync());
        }

        // GET: api/CommandQueries/Commands
        [HttpGet("Commands")]
        public async Task<IActionResult> GetCommands()
        {
            if (context.CommandQueries == null)
            {
                return NotFound();
            }
            var commands = await context.CommandQueries.Where(x => x.IsCommand).ToListAsync();
            return Ok(commands);
        }

        // GET: api/CommandQueries/Queries
        [HttpGet("Queries")]
        public async Task<IActionResult> GetQueries()
        {
            if (context.CommandQueries == null)
            {
                return NotFound();
            }
            var queries = await context.CommandQueries.Where(x => !x.IsCommand).ToListAsync();
            return Ok(queries);
        }

        // GET: api/CommandQueries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommandQuery(long id)
        {
            if (context.CommandQueries == null)
            {
                return NotFound();
            }
            var command = await context.CommandQueries.Include(x => x.CommandApprovalFlowConfiguration).FirstOrDefaultAsync(x => x.Id == id);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(command);
        }

        private bool CommandExists(long id)
        {
            return (context.CommandQueries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}