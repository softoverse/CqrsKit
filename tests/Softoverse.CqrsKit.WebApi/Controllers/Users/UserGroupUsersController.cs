using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.User;
using Softoverse.CqrsKit.WebApi.Services;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupUsersController(ApplicationDbContext dbContext, ICacheService cacheService) : ControllerBase
    {
        // GET: api/UserGroupUsers
        [HttpGet]
        public async Task<IActionResult> GetUserGroupUsers([FromQuery] long? userGroupId)
        {
            if (userGroupId == null || userGroupId == 0)
            {
                return Ok(await dbContext.UserGroupUsers.ToListAsync());
            }
            return Ok(await dbContext.UserGroupUsers.Where(x => x.UserGroupId == userGroupId).ToListAsync());
        }

        // GET: api/UserGroupUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroupUser>> GetUserGroupUser(long id)
        {
            var userGroupUser = await dbContext.UserGroupUsers.FindAsync(id);

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

            dbContext.Entry(userGroupUser).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
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
            dbContext.UserGroupUsers.Add(userGroupUser);
            await dbContext.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return CreatedAtAction("GetUserGroupUser", new
            {
                id = userGroupUser.Id
            }, userGroupUser);
        }

        [HttpPost("range")]
        public async Task<ActionResult<UserGroupUser>> PostUserGroupUserRange(List<UserGroupUser> userGroupUsers)
        {
            var userGroupIds = userGroupUsers.Select(x => x.UserGroupId);

            var userGroupUserIdsToKeepInDb = userGroupUsers.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            await dbContext.UserGroupUsers.Where(x => !userGroupUserIdsToKeepInDb.Contains(x.Id) && userGroupIds.Contains(x.UserGroupId)).ExecuteDeleteAsync();

            await dbContext.UserGroupUsers.AddRangeAsync(userGroupUsers.Where(x => x.Id == 0));
            await dbContext.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return Ok(userGroupUsers);
        }

        // DELETE: api/UserGroupUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGroupUser(long id)
        {
            if (dbContext.UserGroupUsers == null)
            {
                return NotFound();
            }
            var userGroupUser = await dbContext.UserGroupUsers.FindAsync(id);
            if (userGroupUser == null)
            {
                return NotFound();
            }

            dbContext.UserGroupUsers.Remove(userGroupUser);
            await dbContext.SaveChangesAsync();

            await cacheService.ClearAllCacheAsync();

            return NoContent();
        }

        private bool UserGroupUserExists(long id)
        {
            return (dbContext.UserGroupUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}