using Softoverse.CqrsKit.WebApi.Extensions;

namespace Softoverse.CqrsKit.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.AddSwagger();

        builder.AddDatabaseConfiguration();
        builder.AddAuthorizationConfiguration();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
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