using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class AuthorizationConfigurationExtension
{
    public static WebApplicationBuilder AddAuthorizationConfiguration(this WebApplicationBuilder builder)
    {
        // Add builder.Services to the container. 

        // // If you want to use authorization like [Authorize("ApiKey")] for JWT & ApiKey both with 1 Attribute
        // builder.Services.AddAuthorization();
        // builder.Services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();

        // If you want to use authorization like [Authorize, Authorize("ApiKey")] for JWT & ApiKey both with different Attributes
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.ApiKeyPolicy, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.AddRequirements(new ApiKeyRequirement());
            });
        });

        builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();
        builder.Services.AddScoped<ApiKeyEndpointFilter>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(builder.Configuration["JWT:UserLockoutMinutes"]));
            options.Lockout.MaxFailedAccessAttempts = 4;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });

        //Adding Authentication - JWT
        builder.Services.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                   // options.DefaultScheme = "Cookies";
                   // options.DefaultChallengeScheme = "OAuth2";
               })
               .AddCookie("Cookies") // Cookie authentication for storing user sessions
               .AddJwtBearer(options =>
               {
                   options.SaveToken = false;
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       RequireExpirationTime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = builder.Configuration["JWT:Issuer"],
                       ValidAudience = builder.Configuration["JWT:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.TryAdd("Token-Expired", "true");
                           }
                           return Task.CompletedTask;
                       }
                   };
               });
        // .AddOAuth("OAuth2", options =>
        // {
        //     options.ClientId = "your-client-id";
        //     options.ClientSecret = "your-client-secret";
        //     options.CallbackPath = "/signin-oauth"; // Redirect URL after authentication
        //     options.AuthorizationEndpoint = "https://example.com/oauth/authorize";
        //     options.TokenEndpoint = "https://example.com/oauth/token";
        //     options.UserInformationEndpoint = "https://example.com/oauth/userinfo";
        //
        //     // Define requested scopes
        //     options.Scope.Add("read");
        //     options.Scope.Add("write");
        //
        //     // Map user claims from OAuth provider response
        //     options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        //     options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        //     options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        //
        //     // Save tokens (optional)
        //     options.SaveTokens = true;
        //
        //     options.Events = new OAuthEvents
        //     {
        //         OnCreatingTicket = async context =>
        //         {
        //             // Example: Fetch user info from provider
        //             var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
        //             request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
        //
        //             var response = await context.Backchannel.SendAsync(request);
        //             if (response.IsSuccessStatusCode)
        //             {
        //                 var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        //                 context.RunClaimActions(user.RootElement);
        //             }
        //         }
        //     };
        // });

        return builder;
    }
}

public class CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? authorizationPolicy = policyName.Trim() switch
        {
            Constants.ApiKeyPolicy => new AuthorizationPolicyBuilder().AddRequirements(new ApiKeyRequirement()).Build(),
            Constants.ApiKeyAndJwtPolicy => new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddRequirements(new ApiKeyRequirement()).Build(),
            null or "" => await base.GetPolicyAsync(policyName),
            _ => await base.GetPolicyAsync(policyName)
        };

        return authorizationPolicy;
    }
}

public class ApiKeyRequirement : IAuthorizationRequirement
{
}

public class ApiKeyHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : AuthorizationHandler<ApiKeyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
// #if DEBUG
//         return Task.CompletedTask;
// #endif

        string? apiKey = httpContextAccessor?.HttpContext?.Request.Headers[Constants.ApiKeyHeaderName].ToString();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (!apiKey.Equals(configuration[Constants.ApiKeyPolicy]))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

public class ApiKeyEndpointFilter(IConfiguration configuration) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
// #if DEBUG
//         return Task.CompletedTask;
// #endif

        string? apiKey = context.HttpContext?.Request.Headers[Constants.ApiKeyHeaderName].ToString();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Results.BadRequest();
        }

        if (!apiKey.Equals(configuration[Constants.ApiKeyPolicy]))
        {
            return Results.Unauthorized();
        }
        return await next(context);
    }
}