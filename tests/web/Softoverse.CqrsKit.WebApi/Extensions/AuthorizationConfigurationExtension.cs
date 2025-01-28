using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class AuthorizationConfigurationExtension
{
    public static WebApplicationBuilder AddAuthorizationConfiguration(this WebApplicationBuilder builder)
    {
        // Add builder.Services to the container.
        builder.Services.AddAuthorization();

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
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });

        //Adding Authentication - JWT
        builder.Services.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                   options.DefaultScheme = "Cookies";
                   options.DefaultChallengeScheme = "OAuth2";
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
               })
               .AddOAuth("OAuth2", options =>
               {
                   options.ClientId = "your-client-id";
                   options.ClientSecret = "your-client-secret";
                   options.CallbackPath = "/signin-oauth"; // Redirect URL after authentication
                   options.AuthorizationEndpoint = "https://example.com/oauth/authorize";
                   options.TokenEndpoint = "https://example.com/oauth/token";
                   options.UserInformationEndpoint = "https://example.com/oauth/userinfo";

                   // Define requested scopes
                   options.Scope.Add("read");
                   options.Scope.Add("write");

                   // Map user claims from OAuth provider response
                   options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                   options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                   options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                   // Save tokens (optional)
                   options.SaveTokens = true;

                   options.Events = new OAuthEvents
                   {
                       OnCreatingTicket = async context =>
                       {
                           // Example: Fetch user info from provider
                           var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                           request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);
                   
                           var response = await context.Backchannel.SendAsync(request);
                           if (response.IsSuccessStatusCode)
                           {
                               var user = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                               context.RunClaimActions(user.RootElement);
                           }
                       }
                   };
               });

        return builder;
    }
}