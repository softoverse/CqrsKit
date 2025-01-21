using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandApprovalFlowConfigurationsController(ApplicationDbContext dbContext) : ControllerBase
    {
        // GET: api/CommandApprovalFlowConfigurations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandApprovalFlowConfiguration>>> GetCommandApprovalFlowConfigurations()
        {
            return Ok(await dbContext.CommandApprovalFlowConfigurations.ToListAsync());
        }

        // GET: api/CommandApprovalFlowConfigurations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommandApprovalFlowConfiguration>> GetCommandApprovalFlowConfiguration(long id)
        {
            var commandApprovalFlowConfiguration = await dbContext.CommandApprovalFlowConfigurations.FindAsync(id);

            return Ok(commandApprovalFlowConfiguration);
        }

        // PUT: api/CommandApprovalFlowConfigurations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCommandApprovalFlowConfiguration(long id, CommandApprovalFlowConfiguration data)
        //{
        //    if (id != data.Id) return BadRequest();
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    _dbContext.CommandApprovalFlowConfigurations.Update(data);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok();
        //}

        // POST: api/CommandApprovalFlowConfigurations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommandApprovalFlowConfiguration>> PostCommandApprovalFlowConfiguration(CommandApprovalFlowConfiguration data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (data.Id == 0)
            {
                await dbContext.CommandApprovalFlowConfigurations.AddAsync(data);
            }
            else
            {
                await dbContext.CommandApprovalFlowConfigurations.Where(x => x.Id == data.Id)
                                                                    .ExecuteUpdateAsync(x => x
                                                                    .SetProperty(y => y.ApprovalFlowConfigurationId, data.ApprovalFlowConfigurationId)
                                                                    .SetProperty(y => y.CommandId, data.CommandId));
            }

            await dbContext.CommandQueries.Where(x => x.Id == data.CommandId).ExecuteUpdateAsync(x => x.SetProperty(y => y.IsApprovalFlowRequired, data.ApprovalFlowConfigurationId != 0));
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommandApprovalFlowConfiguration(long id)
        {
            var data = await dbContext.CommandApprovalFlowConfigurations.FindAsync(id);
            if (data == null) return NotFound();

            await dbContext.CommandQueries.Where(x => x.Id == data.CommandId).ExecuteUpdateAsync(x => x.SetProperty(y => y.IsApprovalFlowRequired, false));
            dbContext.CommandApprovalFlowConfigurations.Remove(data);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}