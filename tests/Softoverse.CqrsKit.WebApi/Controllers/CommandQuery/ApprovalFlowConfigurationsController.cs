using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.CQRS.Custom;

namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalFlowConfigurationsController(ApplicationDbContext dbContext) : ControllerBase
    {
        // GET: api/ApprovalFlowConfigurations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalFlowConfiguration>>> GetApprovalFlowConfigurations(CancellationToken ct = default)
        {
            return Ok(await dbContext.ApprovalFlowConfigurations.ToListAsync(ct));
        }

        // GET: api/ApprovalFlowConfigurations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalFlowConfiguration>> GetApprovalFlowConfiguration(long id, CancellationToken ct = default)
        {
            var approvalFlowConfiguration = await dbContext.ApprovalFlowConfigurations.Include(x => x.ApprovalFlowConfigurationSteps).FirstOrDefaultAsync(x => x.Id == id, ct);
            return Ok(approvalFlowConfiguration);
        }

        // PUT: api/ApprovalFlowConfigurations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalFlowConfiguration(long id, ApprovalFlowConfiguration data, CancellationToken ct = default)
        {
            if (id != data.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var approvalFlowConfigurationDetailIdsToKeepInDb = data.ApprovalFlowConfigurationSteps.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            await dbContext.ApprovalFlowConfigurationSteps
                                                    .Where(x => x.ApprovalFlowConfigurationId == id && !approvalFlowConfigurationDetailIdsToKeepInDb.Contains(x.Id))
                                                    .ExecuteDeleteAsync(ct);

            dbContext.ApprovalFlowConfigurations.Update(data);

            await dbContext.SaveChangesAsync(ct);

            return Ok();
        }

        // POST: api/ApprovalFlowConfigurations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApprovalFlowConfiguration>> PostApprovalFlowConfiguration(ApprovalFlowConfiguration data, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await dbContext.ApprovalFlowConfigurations.AddAsync(data, ct);
            await dbContext.SaveChangesAsync(ct);

            return Ok();
        }

        // DELETE: api/ApprovalFlowConfigurations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalFlowConfiguration(long id, CancellationToken ct = default)
        {
            await dbContext.ApprovalFlowConfigurations.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
            await dbContext.SaveChangesAsync(ct);

            return Ok();
        }
    }
}