using FluentValidation;

using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Extensions;

namespace Softoverse.CqrsKit.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.AddSwaggerConfiguration();
        builder.AddDatabaseConfiguration();
        builder.AddAuthorizationConfiguration();
        
        // FluentValidation validators needs to be registered as singleton
        builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

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
        
        app.MapGet("/", () => "Hello world").RequireAuthorization();

        await app.RunAsync();
    }
}