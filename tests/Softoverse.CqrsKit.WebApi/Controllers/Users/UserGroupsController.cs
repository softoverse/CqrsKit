using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Models.User;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController(ApplicationDbContext dbContext) : ControllerBase
    {
        // GET: api/UserGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups()
        {
            return await dbContext.UserGroups.ToListAsync();
        }

        // GET: api/UserGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroup>> GetUserGroup(long id)
        {
            var userGroup = await dbContext.UserGroups.FindAsync(id);

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

            dbContext.Entry(userGroup).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
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
            return NoContent();
        }

        // POST: api/UserGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserGroup>> PostUserGroup(UserGroup userGroup)
        {
            dbContext.UserGroups.Add(userGroup);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction("GetUserGroup", new { id = userGroup.Id }, userGroup);
        }

        // DELETE: api/UserGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserGroup(long id)
        {
            var userGroup = await dbContext.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }

            dbContext.UserGroups.Remove(userGroup);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool UserGroupExists(long id)
        {
            return (dbContext.UserGroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}