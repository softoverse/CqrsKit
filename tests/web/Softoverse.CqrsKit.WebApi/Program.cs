using FluentValidation;

using Microsoft.AspNetCore.Mvc.ApplicationParts;

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
               .AddDatabaseConfiguration()
               .AddAuthorizationConfiguration();

        // FluentValidation validators need to be registered as singleton
        builder.Services
               .AddValidatorsFromAssemblyContaining<IWebApiMarker>(ServiceLifetime.Singleton)
               .AddValidatorsFromAssemblyContaining<IWebApiDataAccessMarker>(ServiceLifetime.Singleton)
               .AddValidatorsFromAssemblyContaining<IWebApiModelsMarker>(ServiceLifetime.Singleton)
               .AddValidatorsFromAssemblyContaining<IWebApiModuleMarker>(ServiceLifetime.Singleton);

        builder.Services.AddCqrsKit(typeof(IWebApiMarker),
                                    typeof(IWebApiDataAccessMarker),
                                    typeof(IWebApiModelsMarker),
                                    typeof(IWebApiModuleMarker));

        builder.Services
               .AddCqrsKit<IWebApiMarker>()
               .AddCqrsKit<IWebApiDataAccessMarker>()
               .AddCqrsKit<IWebApiModelsMarker>()
               .AddCqrsKit<IWebApiModuleMarker>();

        var app = builder.Build();

        await app.SeedApplicationBaseDataAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => "Hello world")
           .RequireAuthorization();

        await app.RunAsync();
    }
}