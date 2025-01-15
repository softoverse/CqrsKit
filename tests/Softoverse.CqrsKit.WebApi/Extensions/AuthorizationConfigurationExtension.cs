using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class AuthorizationConfigurationExtension
{
    public static void AddAuthorizationConfiguration(this WebApplicationBuilder builder)
    {
        // Add builder.Services to the container.
        builder.Services.AddAuthorization();

        //Adding Authentication - JWT
        builder.Services.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               })
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
               });
    }
}