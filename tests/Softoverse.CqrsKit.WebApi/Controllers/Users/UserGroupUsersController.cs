using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.User;
using Softoverse.CqrsKit.WebApi.Services;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupUsersController(ApplicationDbContext context, ICacheService cacheService) : ControllerBase
    {
        // GET: api/UserGroupUsers
        [HttpGet]
        public async Task<IActionResult> GetUserGroupUsers([FromQuery] long? userGroupId)
        {
            if (userGroupId == null || userGroupId == 0)
            {
                return Ok(await context.UserGroupUsers.ToListAsync());
            }
            return Ok(await context.UserGroupUsers.Where(x => x.UserGroupId == userGroupId).ToListAsync());
        }

        // GET: api/UserGroupUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroupUser>> GetUserGroupUser(long id)
        {
            if (context.UserGroupUsers == null)
            {
                return NotFound();
            }
            var userGroupUser = await context.UserGroupUsers.FindAsync(id);

            if (userGroupUser == null)
            {
                return NotFound();
            }

            return userGroupUser;
        }

        // PUT: api/UserGroupUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserGroupUser(long id, UserGroupUser userGroupUser)
        {
            if (id != userGroupUser.Id)
            {
                return BadRequest();
            }

            context.Entry(userGroupUser).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGroupUserExists(id))
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

        // POST: api/UserGroupUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserGroupUser>> PostUserGroupUser(UserGroupUser userGroupUser)
        {
            if (context.UserGroupUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserGroupUsers'  is null.");
            }
            context.UserGroupUsers.Add(userGroupUser);
            await context.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return CreatedAtAction("GetUserGroupUser", new { id = userGroupUser.Id }, userGroupUser);
        }

        [HttpPost("range")]
        public async Task<ActionResult<UserGroupUser>> PostUserGroupUserRange(List<UserGroupUser> userGroupUsers)
        {
            var userGroupIds = userGroupUsers.Select(x => x.UserGroupId);

            var userGroupUserIdsToKeepInDb = userGroupUsers.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            await context.UserGroupUsers.Where(x => !userGroupUserIdsToKeepInDb.Contains(x.Id) && userGroupIds.Contains(x.UserGroupId)).ExecuteDeleteAsync();

            await context.UserGroupUsers.AddRangeAsync(userGroupUsers.Where(x => x.Id == 0));
            await context.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return Ok(userGroupUsers);
        }

        // DELETE: api/UserGroupUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGroupUser(long id)
        {
            if (context.UserGroupUsers == null)
            {
                return NotFound();
            }
            var userGroupUser = await context.UserGroupUsers.FindAsync(id);
            if (userGroupUser == null)
            {
                return NotFound();
            }

            context.UserGroupUsers.Remove(userGroupUser);
            await context.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return NoContent();
        }

        private bool UserGroupUserExists(long id)
        {
            return (context.UserGroupUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}