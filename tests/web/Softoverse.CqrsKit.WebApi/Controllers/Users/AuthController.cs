﻿using System.Collections.Concurrent;
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
    private readonly int _accessTokenExpiresIn = Convert.ToInt32(configuration["JWT:AccessTokenExpirationMinutes"]);
    private readonly int _refreshTokenExpiresIn = Convert.ToInt32(configuration["JWT:RefreshTokenExpirationMinutes"]);

    private static readonly ConcurrentDictionary<string, string> RefreshTokens = new ConcurrentDictionary<string, string>();

    // POST api/Auth/token
    [HttpPost("token")]
    public async Task<IActionResult> Token(TokenRequest request)
    {
        var username = GetUsernameFromRefreshToken(request.RefreshToken);
        string? currentRefreshToken;

        switch (request.GrantType)
        {
            case "password" when (IsBasicHeaderValid(request.ClientId, request.ClientSecret) || IsBasicHeaderValid(request.Authorization!) || IsBasicHeaderValid(HttpContext)):
                {
                    string accessToken = GenerateApiUserToken(request.Username!);
                    currentRefreshToken = GenerateRefreshToken(request.Username!);

                    var user = await userManager.FindByNameAsync(request.Username!);
                    var signInResult = await signInManager.PasswordSignInAsync(user!, request.Password!, false, false);

                    if (signInResult.Succeeded)
                    {
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

                    RefreshTokens.Remove(request.Username!, out string? _);
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
                    RefreshTokens.Remove(request.Username!, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Client Id or Client Secret."
                    });
                }
            case "refresh_token":
                {
                    RefreshTokens.Remove(username!, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Refresh Token."
                    });
                }
            default:
                {
                    RefreshTokens.Remove(username!, out string? _);
                    return Unauthorized(new
                    {
                        message = "Invalid Request."
                    });
                }
        }

    }

    // POST api/Auth/register
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
        string signingKey = configuration["JWT:Key"] ?? "";
        string? issuer = configuration["JWT:Issuer"];
        string? audience = configuration["JWT:Audience"];
        DateTime expireDateTime = DateTime.UtcNow.AddMinutes(_accessTokenExpiresIn);

        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
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