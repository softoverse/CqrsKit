using FluentValidation;

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
            var apiKey = context.RequestServices.GetRequiredService<IConfiguration>()["ApiKey"];

            if (context.Request.Path.StartsWithSegments("/swagger/index.html") || context.Request.Path.StartsWithSegments("/scalar/v1"))
            {
                if (!context.Request.Query.TryGetValue("apiKey", out var extractedApiKey) || extractedApiKey != apiKey)
                {
                    // Redirect to another API endpoint (e.g., "/unauthorized")
                    context.Response.Redirect("/unauthorized");
                    return;
                }
                // if (context.Request.Path.StartsWithSegments("/swagger"))
                // {
                //     context.Response.Redirect("/swagger");
                //     return;
                // }
                // if (context.Request.Path.StartsWithSegments("/scalar"))
                // {
                //     context.Response.Redirect("/scalar");
                //     return;
                // }


            }

            await next();
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUi();
            app.MapScalar();
        }

        app.UseStaticFiles();
        app.MapGet("/doc", () => Results.Redirect("/doc.html"));
        app.MapGet("/unauthorized", () => Results.Redirect("/unauthorized.html"));
        // app.MapGet("/swagger", () => Results.Redirect("/swagger/index.html"));
        // app.MapGet("/scalar", () => Results.Redirect("/scalar/v1"));

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}