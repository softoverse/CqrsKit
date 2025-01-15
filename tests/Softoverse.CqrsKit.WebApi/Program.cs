using Softoverse.CqrsKit.WebApi.Extensions;

namespace Softoverse.CqrsKit.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.AddSwaggerConfiguration();
        builder.AddDatabaseConfiguration();
        builder.AddAuthorizationConfiguration();

        var app = builder.Build();

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

        app.Run();
    }
}