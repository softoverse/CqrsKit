// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// using Softoverse.CqrsKit.WebApi.DataAccess;
// using Softoverse.CqrsKit.WebApi.Models.CQRS.Custom;
//
// namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class ApprovalFlowConfigurationStepsController(ApplicationDbContext dbContext) : ControllerBase
//     {
//         // GET: api/ApprovalFlowConfigurationSteps
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<ApprovalFlowConfigurationStep>>> GetApprovalFlowConfigurationSteps(CancellationToken ct = default)
//         {
//             return Ok(await dbContext.ApprovalFlowConfigurationSteps.ToListAsync(ct));
//         }
//
//         // GET: api/ApprovalFlowConfigurationSteps/5
//         [HttpGet("{id}")]
//         public async Task<ActionResult<ApprovalFlowConfigurationStep>> GetApprovalFlowConfigurationStep(long id, CancellationToken ct = default)
//         {
//             return Ok(await dbContext.ApprovalFlowConfigurationSteps.FindAsync(id, ct));
//         }
//
//         // PUT: api/ApprovalFlowConfigurationSteps/5
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPut("{id}")]
//         public async Task<IActionResult> PutApprovalFlowConfigurationStep(long id, ApprovalFlowConfigurationStep data, CancellationToken ct = default)
//         {
//             if (id != data.Id) return BadRequest();
//             if (!ModelState.IsValid) return BadRequest(ModelState);
//
//             dbContext.ApprovalFlowConfigurationSteps.Update(data);
//             await dbContext.SaveChangesAsync(ct);
//
//             return Ok();
//         }
//
//         // POST: api/ApprovalFlowConfigurationSteps
//         // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//         [HttpPost]
//         public async Task<ActionResult<ApprovalFlowConfigurationStep>> PostApprovalFlowConfigurationStep(ApprovalFlowConfigurationStep data, CancellationToken ct = default)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState);
//
//             await dbContext.ApprovalFlowConfigurationSteps.AddAsync(data, ct);
//             await dbContext.SaveChangesAsync(ct);
//
//             return Ok();
//         }
//
//         // DELETE: api/ApprovalFlowConfigurationSteps/5
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteApprovalFlowConfigurationStep(long id, CancellationToken ct = default)
//         {
//             await dbContext.ApprovalFlowConfigurationSteps.Where(x => x.Id == id).ExecuteDeleteAsync(ct);
//             await dbContext.SaveChangesAsync(ct);
//
//             return Ok();
//         }
//     }
// }