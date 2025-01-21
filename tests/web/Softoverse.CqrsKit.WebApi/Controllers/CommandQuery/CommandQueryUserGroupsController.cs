using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.CQRS.MappingModels;

namespace Softoverse.CqrsKit.WebApi.Controllers.CommandQuery
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandQueryUserGroupsController(ApplicationDbContext dbContext) : ControllerBase
    {
        // GET: api/CommandQueryUserGroups
        [HttpGet]
        public async Task<IActionResult> GetCommandQueryUserGroups([FromQuery] long? userGroupId)
        {
            if (userGroupId == null || userGroupId == 0)
            {
                return Ok(await dbContext.CommandQueryUserGroups.ToListAsync());
            }
            return Ok(await dbContext.CommandQueryUserGroups.Where(x => x.UserGroupId == userGroupId).ToListAsync());
        }

        // GET: api/CommandQueryUserGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommandQueryUserGroup(long id)
        {
            var commandQueryUserGroup = await dbContext.CommandQueryUserGroups.FindAsync(id);

            if (commandQueryUserGroup == null)
            {
                return NotFound();
            }

            return Ok(commandQueryUserGroup);
        }

        [HttpPost("commands/range")]
        public async Task<IActionResult> PostCommandUserGroupRange(List<CommandQueryUserGroup> commandQueryUserGroups, [FromQuery] long userGroupId)
        {
            if (userGroupId == 0)
            {
                return BadRequest("User Group not provided");
            }

            var userGroupIds = commandQueryUserGroups.Select(x => x.UserGroupId);

            var commandQueryUserGroupIdsToKeepInDb = commandQueryUserGroups.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            var deleteAll = commandQueryUserGroups == null || commandQueryUserGroups.Count == 0;

            await dbContext.CommandQueryUserGroups
                           .Where(x =>
                                        x.CommandQuery!.IsCommand
                                    &&
                                      (deleteAll ? x.UserGroupId == userGroupId : userGroupIds.Contains(x.UserGroupId) && !commandQueryUserGroupIdsToKeepInDb.Contains(x.Id))
                                 )
                           .Include(x => x.CommandQuery)
                           .ExecuteDeleteAsync();
            //await _context.SaveChangesAsync();

            await dbContext.CommandQueryUserGroups.AddRangeAsync(commandQueryUserGroups!.Where(x => x.Id == 0));
            await dbContext.SaveChangesAsync();

            return Ok(commandQueryUserGroups);
        }

        [HttpPost("queries/range")]
        public async Task<IActionResult> PostQueryUserGroupRange(List<CommandQueryUserGroup> commandQueryUserGroups, [FromQuery] long userGroupId)
        {
            var userGroupIds = commandQueryUserGroups.Select(x => x.UserGroupId);

            var commandQueryUserGroupIdsToKeepInDb = commandQueryUserGroups.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            var deleteAll = commandQueryUserGroups == null || commandQueryUserGroups.Count == 0;

            await dbContext.CommandQueryUserGroups
                           .Where(x =>
                                        !x.CommandQuery!.IsCommand
                                    &&
                                      (deleteAll ? x.UserGroupId == userGroupId : userGroupIds.Contains(x.UserGroupId) && !commandQueryUserGroupIdsToKeepInDb.Contains(x.Id))
                                 )
                           .Include(x => x.CommandQuery)
                           .ExecuteDeleteAsync();
            //await _context.SaveChangesAsync();

            await dbContext.CommandQueryUserGroups.AddRangeAsync(commandQueryUserGroups!.Where(x => x.Id == 0));
            await dbContext.SaveChangesAsync();

            return Ok(commandQueryUserGroups);
        }

        // DELETE: api/CommandQueryUserGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommandQueryUserGroup(long id)
        {
            var commandQueryUserGroup = await dbContext.CommandQueryUserGroups.FindAsync(id);
            if (commandQueryUserGroup == null)
            {
                return NotFound();
            }

            dbContext.CommandQueryUserGroups.Remove(commandQueryUserGroup);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}