using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Softoverse.CqrsKit.WebApi.Models.ViewModels;

namespace Softoverse.CqrsKit.WebApi.Controllers.Users;

[Route("api/auth")]
[ApiController]
public class AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration) : ControllerBase
{
    private readonly int _accessTokenExpiresIn = Convert.ToInt32(configuration["JWT:AccessTokenExpirationMinutes"]);
    private readonly int _refreshTokenExpiresIn = Convert.ToInt32(configuration["JWT:RefreshTokenExpirationMinutes"]);
    private readonly int _userLockoutMinutes = Convert.ToInt32(configuration["JWT:UserLockoutMinutes"]);

    private static readonly ConcurrentDictionary<string, string> RefreshTokens = new ConcurrentDictionary<string, string>();

    // POST api/Auth/token
    [HttpPost("token")]
    public async Task<IActionResult> Token(TokenRequest request)
    {
        if (string.IsNullOrEmpty(request.Username))
        {
            return Unauthorized(new
            {
                message = "Invalid Username or Password."
            });
        }

        var user = await GetIdentityUser(request.Username!);
        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid Username or Password."
            });
        }

        var username = GetUsernameFromRefreshToken(request.RefreshToken);
        string? currentRefreshToken;

        switch (request.GrantType)
        {
            case "password" when (IsBasicHeaderValid(request.ClientId, request.ClientSecret) || IsBasicHeaderValid(request.Authorization!) || IsBasicHeaderValid(HttpContext)):
                {
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        var lockoutEndDate = await userManager.GetLockoutEndDateAsync(user);
                        return Unauthorized(new
                        {
                            message = $"Account is still locked. Try again after {lockoutEndDate.ToString()}"
                        });
                    }

                    var signInResult = await signInManager.PasswordSignInAsync(user, request.Password!, false, false);
                    if (signInResult.Succeeded)
                    {
                        _ = userManager.ResetAccessFailedCountAsync(user);
                        _ = userManager.SetLockoutEnabledAsync(user, false);
                        _ = userManager.SetLockoutEndDateAsync(user, null);

                        string accessToken = GenerateApiUserToken(user.UserName!);
                        currentRefreshToken = GenerateRefreshToken(user.UserName!);

                        return Ok(new TokenResponse
                        {
                            Message = "Login Successful.",
                            RefreshToken = currentRefreshToken,
                            AccessToken = accessToken,
                            TokenType = "Bearer",
                            ExpiresIn = TimeSpan.FromMinutes(_accessTokenExpiresIn).TotalSeconds,
                            RefreshExpiresIn = TimeSpan.FromMinutes(_refreshTokenExpiresIn).TotalSeconds,
                            Login = new TokenDetails
                            {
                                AccessToken = accessToken,
                                TokenType = "Bearer"
                            }
                        });
                    }

                    await userManager.AccessFailedAsync(user);

                    var accessFailedCount = await userManager.GetAccessFailedCountAsync(user);
                    if (accessFailedCount > 3)
                    {
                        await userManager.SetLockoutEnabledAsync(user, true);

                        var lockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(_userLockoutMinutes);
                        await userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
                        return Unauthorized(new
                        {
                            message = $"Too many try with invalid credentials. Account is locked. Try again after {lockoutEndDate.ToString()}"
                        });
                    }

                    RefreshTokens.Remove(user.UserName!, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Username or Password."
                    });
                }
            case "refresh_token" when RefreshTokens.TryGetValue(username, out currentRefreshToken):
                {
                    string accessToken = GenerateApiUserToken(username);
                    return Ok(new TokenResponse
                    {
                        Message = "Token Refresh Successful.",
                        RefreshToken = currentRefreshToken,
                        AccessToken = accessToken,
                        TokenType = "Bearer",
                        ExpiresIn = TimeSpan.FromMinutes(_accessTokenExpiresIn).TotalSeconds,
                        RefreshExpiresIn = TimeSpan.FromMinutes(_refreshTokenExpiresIn).TotalSeconds,
                        Login = new TokenDetails
                        {
                            AccessToken = accessToken,
                            TokenType = "Bearer"
                        }
                    });
                }
            case "password":
                {
                    RefreshTokens.Remove(user.UserName!, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Client Id or Client Secret."
                    });
                }
            case "refresh_token":
                {
                    RefreshTokens.Remove(username, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Refresh Token."
                    });
                }
            default:
                {
                    RefreshTokens.Remove(username, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Request."
                    });
                }
        }

    }

    // POST api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistration userRegistration)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        IdentityUser identityUser = new IdentityUser
        {
            UserName = userRegistration.Username,
            Email = userRegistration.Email
        };

        var userResult = await userManager.CreateAsync(identityUser, userRegistration.Password);

        return Ok(new
        {
            message = userResult.Succeeded ? "Registered Successfully." : "Registration Failed.",
            errors = userResult.Errors
        });
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

    private bool IsBasicHeaderValid(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues value))
        {
            return false;
        }

        try
        {
            // decoding authToken we get decode value in 'Username:Password' format
            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(value!);
            return IsBasicHeaderValid(authenticationHeaderValue.Parameter!);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private bool IsBasicHeaderValid(string basicHeader)
    {
        try
        {
            if (basicHeader.Contains(' '))
            {
                basicHeader = basicHeader.Split(' ')[1];
            }

            var bytes = Convert.FromBase64String(basicHeader);
            var decodedString = Encoding.UTF8.GetString(bytes);

            // splitting decodeAuthToken using ':'
            var splitText = decodedString.Split([':']);

            string clientId = splitText[0];
            string clientSecret = splitText[1];
            var isValidBasicHeader = IsBasicHeaderValid(clientId, clientSecret);

            return isValidBasicHeader;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private bool IsBasicHeaderValid(string? clientId, string? clientSecret)
    {
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            return false;
        }

        return clientId == configuration["JWT:ClientId"] && clientSecret == configuration["JWT:ClientSecret"];
    }

    private string GenerateApiUserToken(string username)
    {
        return GenerateApiUserToken(username, configuration["JWT:Key"], configuration["JWT:Issuer"], configuration["JWT:Audience"], _accessTokenExpiresIn);
    }

    public static string GenerateApiUserToken(string username, string? signingKey, string? issuer, string? audience, int accessTokenExpiresIn)
    {
        DateTime expireDateTime = DateTime.UtcNow.AddMinutes(accessTokenExpiresIn);

        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey!);
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(signingKeyBytes);
        SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var authClaims = new List<Claim>
        {
            new Claim("username", username)
        };

        JwtSecurityToken token = new JwtSecurityToken(issuer,
                                                      audience,
                                                      authClaims,
                                                      DateTime.UtcNow,
                                                      expireDateTime,
                                                      signingCredentials);
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

    private static string GenerateRefreshToken(string username)
    {
        if (!RefreshTokens.TryGetValue(username, out string? currentRefreshToken))
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            currentRefreshToken = ToBase64String(username, Convert.ToBase64String(randomNumber));

            RefreshTokens.GetOrAdd(username, ToBase64String(username, currentRefreshToken));
        }
        ArgumentNullException.ThrowIfNull(currentRefreshToken);
        return currentRefreshToken;
    }

    private static string ToBase64String(string username, string refreshToken)
    {
        return StringToBase64($"{refreshToken}::::{username}");
    }

    private static string GetUsernameFromRefreshToken(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) return string.Empty;
        var normalString = Base64ToString(refreshToken);
        return normalString.Split("::::")[1];
    }

    // Method to convert a normal string to a Base64 string
    private static string StringToBase64(string normalString)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(normalString);
        return Convert.ToBase64String(bytes);
    }

    // Method to convert a Base64 string back to a normal string
    private static string Base64ToString(string base64String)
    {
        byte[] bytes = Convert.FromBase64String(base64String);
        return Encoding.UTF8.GetString(bytes);
    }

    #endregion Non Action Methods
}