#if DEBUG

using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users;

[Route("api")]
[ApiController]
public class AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration) : ControllerBase
{
    private readonly int _accessTokenExpiresIn = Convert.ToInt32(configuration["JWT:AccessTokenExpirationMinutes"]);
    private readonly int _userLockoutMinutes = Convert.ToInt32(configuration["JWT:UserLockoutMinutes"]);

    [HttpPost("login")]
    public async Task<IActionResult> Token([FromBody] UserLogin request)
    {
        if (string.IsNullOrEmpty(request.Username))
        {
            return Unauthorized("Invalid Username or Password.");
        }
        var user = await GetIdentityUser(request.Username!);

        if (user is null)
        {
            return Unauthorized("Invalid Username or Password.");
        }

        if (await userManager.IsLockedOutAsync(user!))
        {
            var lockoutEndDate = await userManager.GetLockoutEndDateAsync(user!);
            return Unauthorized(new
            {
                message = $"Account is still locked. Try again after {lockoutEndDate.ToString()}"
            });
        }

        var signInResult = await signInManager.PasswordSignInAsync(user!, request.Password!, false, false);
        if (signInResult.Succeeded)
        {
            _ = userManager.ResetAccessFailedCountAsync(user!);
            _ = userManager.SetLockoutEnabledAsync(user!, false);
            _ = userManager.SetLockoutEndDateAsync(user!, null);
            string accessToken = GenerateApiUserToken(request.Username!);
            return Ok(accessToken);
        }

        await userManager.AccessFailedAsync(user!);

        var accessFailedCount = await userManager.GetAccessFailedCountAsync(user!);
        if (accessFailedCount > 3)
        {
            await userManager.SetLockoutEnabledAsync(user!, true);
            var lockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(_userLockoutMinutes);
            await userManager.SetLockoutEndDateAsync(user!, lockoutEndDate);
            return Unauthorized($"Too many try with invalid credentials. Account is locked. Try again after {lockoutEndDate.ToString()}");
        }

        return Unauthorized("Invalid Username or Password.");
    }

    // POST api/Auth/lockout
    [HttpPost("lockout")]
    public async Task<IActionResult> Lockout([FromForm] string username)
    {
        var user = await GetIdentityUser(username);
        await userManager.SetLockoutEnabledAsync(user!, true);
        await userManager.SetLockoutEndDateAsync(user!, DateTimeOffset.UtcNow.AddMinutes(_userLockoutMinutes));

        return Ok("Locked Out.");
    }

    // POST api/Auth/release-lockout
    [HttpPost("release-lockout")]
    public async Task<IActionResult> ReleaseLockout([FromForm] string username)
    {
        var user = await GetIdentityUser(username);
        await userManager.ResetAccessFailedCountAsync(user!);
        await userManager.SetLockoutEnabledAsync(user!, false);
        await userManager.SetLockoutEndDateAsync(user!, null);

        return Ok("Released Lockout.");
    }

    #region Non Action Methods

    private Task<IdentityUser?> GetIdentityUser(string username)
    {
        if (username.Contains('@') && username.Contains('.'))
        {
            return userManager.FindByEmailAsync(username);
        }
        else
        {
            return userManager.FindByNameAsync(username);
        }
    }

    private string GenerateApiUserToken(string username)
    {
        return AuthenticationController.GenerateApiUserToken(username, configuration["JWT:Key"], configuration["JWT:Issuer"], configuration["JWT:Audience"], _accessTokenExpiresIn);
    }

    #endregion Non Action Methods
}

public class UserLogin
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
#endif