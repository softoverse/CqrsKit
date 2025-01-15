using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.User;
using Softoverse.CqrsKit.WebApi.Services;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController(ApplicationDbContext context, ICacheService cacheService) : ControllerBase
    {
        // GET: api/UserGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups()
        {
            if (context.UserGroups == null)
            {
                return NotFound();
            }
            return await context.UserGroups.ToListAsync();
        }

        // GET: api/UserGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroup>> GetUserGroup(long id)
        {
            if (context.UserGroups == null)
            {
                return NotFound();
            }
            var userGroup = await context.UserGroups.FindAsync(id);

            if (userGroup == null)
            {
                return NotFound();
            }

            return userGroup;
        }

        // PUT: api/UserGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserGroup(long id, UserGroup userGroup)
        {
            if (id != userGroup.Id)
            {
                return BadRequest();
            }

            context.Entry(userGroup).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await cacheService.ClearAllCacheAsync();
            return NoContent();
        }

        // POST: api/UserGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserGroup>> PostUserGroup(UserGroup userGroup)
        {
            if (context.UserGroups == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserGroups' is null.");
            }
            context.UserGroups.Add(userGroup);
            await context.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return CreatedAtAction("GetUserGroup", new { id = userGroup.Id }, userGroup);
        }

        // DELETE: api/UserGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGroup(long id)
        {
            if (context.UserGroups == null)
            {
                return NotFound();
            }
            var userGroup = await context.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            context.UserGroups.Remove(userGroup);
            await context.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return NoContent();
        }

        private bool UserGroupExists(long id)
        {
            return (context.UserGroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}