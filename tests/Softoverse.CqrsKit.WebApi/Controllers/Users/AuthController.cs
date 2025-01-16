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

[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration) : ControllerBase
{
    public static ConcurrentDictionary<string, string> RefreshTokens = new ConcurrentDictionary<string, string>();

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromForm] TokenRequest request)
    {
        switch (request.GrantType)
        {
            case "password" when IsBasicHeaderValid(request.ClientId, request.ClientSecret) || IsBasicHeaderValid(HttpContext):
                {
                    string accessToken = GenerateApiUserToken(configuration, request.Username!);
                    string currentRefreshToken = GenerateRefreshToken(request.Username!);

                    var user = await userManager.FindByNameAsync(request.Username!);
                    var signInResult = await signInManager.PasswordSignInAsync(user!, request.Password!, false, false);

                    if (signInResult.Succeeded)
                    {
                        return Ok(new TokenResponse
                        {
                            Message = "Login Successful.",
                            RefreshToken = currentRefreshToken,
                            AccessToken = accessToken,
                            ExpiresIn = 60,
                            Login = new TokenDetails
                            {
                                AccessToken = accessToken,
                                TokenType = "Bearer"
                            }
                        });
                    }
                    else
                    {
                        RefreshTokens.Remove(request.Username!, out string? _);
                        return Unauthorized(new
                        {
                            message = "Invalid Username or Password."
                        });
                    }
                }
            case "password":
                RefreshTokens.Remove(request.Username!, out string? _);
                return Unauthorized(new
                {
                    message = "Invalid Client Id or Client Secret."
                });
            case "refresh_token" when RefreshTokens.TryGetValue(request.Username!, out string? currentRefreshToken):
                {
                    string accessToken = GenerateApiUserToken(configuration, request.Username!);
                    return Ok(new TokenResponse
                    {
                        Message = "Login Successful.",
                        RefreshToken = currentRefreshToken,
                        AccessToken = accessToken,
                        ExpiresIn = 60,
                        Login = new TokenDetails
                        {
                            AccessToken = accessToken,
                            TokenType = "Bearer"
                        }
                    });
                }
            case "refresh_token":
                RefreshTokens.Remove(request.Username!, out string? _);
                return Unauthorized(new
                {
                    message = "Invalid Request."
                });
            default:
                RefreshTokens.Remove(request.Username!, out string? _);
                return Unauthorized(new
                {
                    message = "Invalid Request."
                });
        }
    }


    // POST api/Users
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        IdentityUser identityUser = new IdentityUser
        {
            UserName = user.Username
        };

        return Ok(await userManager.CreateAsync(identityUser, user.Password));
    }

    #region Non Action Methods

    private bool IsBasicHeaderValid(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            return false;
        }
        else
        {
            try
            {
                // decoding authToken we get decode value in 'Username:Password' format
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]!);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter!);
                var decodedString = Encoding.UTF8.GetString(bytes);

                // spliting decodeauthToken using ':'
                var splittedText = decodedString.Split([':']);

                string clientId = splittedText[0];
                string clientSecret = splittedText[1];
                var isValidBasicHeader = clientId == configuration["ClientId"] && clientSecret == configuration["ClientSecret"];

                return isValidBasicHeader;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    private bool IsBasicHeaderValid(string? clientId, string? clientSecret)
    {
        if (string.IsNullOrEmpty(clientId) && string.IsNullOrEmpty(clientSecret))
        {
            return false;
        }

        var isValidBasicHeader = clientId == configuration["ClientId"] && clientSecret == configuration["ClientSecret"];

        return isValidBasicHeader;
    }

    private static string GenerateApiUserToken(IConfiguration configuration, string username)
    {
        string signingKey = configuration["JWT:Key"] ?? "";
        string? issuer = configuration["JWT:Issuer"];
        string? audience = configuration["JWT:Audience"];
        DateTime expireDateTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT:DurationInMinutes"]));

        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
        SymmetricSecurityKey secKey = new SymmetricSecurityKey(signingKeyBytes);
        SigningCredentials creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

        var authClaims = new List<Claim>
        {
            new Claim("username", username)
        };

        JwtSecurityToken token = new JwtSecurityToken(
                                                      issuer: issuer,
                                                      audience: audience,
                                                      claims: authClaims,
                                                      expires: expireDateTime,
                                                      signingCredentials: creds
                                                     );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string writtenToken = handler.WriteToken(token);

        return writtenToken;
    }

    private string GenerateRefreshToken(string username)
    {
        if (RefreshTokens.TryGetValue(username, out string? currentRefreshToken))
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            currentRefreshToken = Convert.ToBase64String(randomNumber);

            RefreshTokens.GetOrAdd(username, currentRefreshToken);
        }
        ArgumentNullException.ThrowIfNull(currentRefreshToken);
        return currentRefreshToken;
    }

    #endregion Non Action Methods
}