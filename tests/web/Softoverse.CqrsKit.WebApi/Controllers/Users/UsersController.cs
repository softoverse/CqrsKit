using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.Model;
using Softoverse.CqrsKit.WebApi.Models.ViewModels;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController(UserManager<IdentityUser> userManager) : ControllerBase
{
    // GET: api/Users
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        List<IdentityUser> users = await userManager.Users.ToListAsync();

        var result = Result<List<IdentityUser>>.Success()
                                               .WithPayload(users)
                                               .WithMessage("Users retrieved successfully");

        return Ok(result);
    }

    // GET api/Users/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        IdentityUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

        var result = Result<IdentityUser?>.Success()
                                          .WithPayload(user)
                                          .WithMessage("User retrieved successfully");

        return Ok(result);
    }

    // PUT api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UserRegistration userRegistration)
    {
        IdentityUser identityUser = new IdentityUser
        {
            UserName = userRegistration.Username,
            Email = userRegistration.Email
        };

        PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
        identityUser.PasswordHash = passwordHasher.HashPassword(identityUser, userRegistration.Password);


        IdentityResult updateResult = await userManager.UpdateAsync(identityUser);

        var result = Result<IdentityResult>.Success()
                                           .WithPayload(updateResult)
                                           .WithMessage("User updated successfully");

        return Ok(result);
    }

    // DELETE api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        IdentityResult deleteResult = await userManager.DeleteAsync(user!);

        var result = Result<IdentityResult>.Success()
                                           .WithPayload(deleteResult)
                                           .WithMessage("User deleted successfully");

        return Ok(result);
    }
}