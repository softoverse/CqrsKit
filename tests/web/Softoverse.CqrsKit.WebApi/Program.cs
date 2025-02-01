using System.Text;

using FluentValidation;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Extensions;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Module;

namespace Softoverse.CqrsKit.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
               .AddControllers(options =>
               {
                   options.ModelValidatorProviders.Clear(); // Disable default asp.net core model state validation
               });

        builder.AddSwaggerConfiguration()
               .AddScalarAuthentication()
               .AddDatabaseConfiguration()
               .AddAuthorizationConfiguration();

        // FluentValidation validators need to be registered as singleton
        builder.Services
               .AddValidatorsFromAssemblyContaining<IWebApiAssemblyMarker>(ServiceLifetime.Singleton)
               .AddValidatorsFromAssemblyContaining<IWebApiDataAccessAssemblyMarker>(ServiceLifetime.Singleton)
               .AddValidatorsFromAssemblyContaining<IWebApiModelsAssemblyMarker>(ServiceLifetime.Singleton)
               .AddValidatorsFromAssemblyContaining<IWebApiModuleAssemblyMarker>(ServiceLifetime.Singleton);

        builder.Services.AddCqrsKit(op =>
        {
            op.EnableLogging = true;

            op.RegisterServicesFromAssemblyContaining<IWebApiAssemblyMarker>()
              .RegisterServicesFromAssemblyContaining<IWebApiDataAccessAssemblyMarker>()
              .RegisterServicesFromAssemblyContaining<IWebApiModelsAssemblyMarker>()
              .RegisterServicesFromAssemblyContaining<IWebApiModuleAssemblyMarker>();
        });

        var app = builder.Build();

        await app.SeedApplicationBaseDataAsync();

        app.Use(async (context, next) =>
        {
            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/scalar"))
            {
                string basicHeader = context.Request.Headers.Authorization.ToString();

                if (string.IsNullOrEmpty(basicHeader))
                {
                    context.Response.StatusCode = 401;
                    context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"http://localhost:5001\"";
                    return;
                }

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

                var isValidBasicHeader = clientId == configuration["JWT:ClientId"] && clientSecret == configuration["JWT:ClientSecret"];

                if (!isValidBasicHeader)
                {
                    context.Response.StatusCode = 401;
                    context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"http://localhost:5001\"";
                    return;
                }
            }

            await next();
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUi();
            app.MapScalar();
        }

        // app.UseStaticFiles();

        app.MapGet("/doc", async () =>
        {
            var filePath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "doc.html");
            var fileContent = await File.ReadAllTextAsync(filePath);
            return Results.Content(fileContent, "text/html");
        });
        app.MapGet("/unauthorized", async () =>
        {
            var filePath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "unauthorized.html");
            var fileContent = await File.ReadAllTextAsync(filePath);
            return Results.Content(fileContent, "text/html");
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}