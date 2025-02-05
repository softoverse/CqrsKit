using FluentValidation;

using Softoverse.CqrsKit.Extensions;
using Softoverse.CqrsKit.WebApi;
using Softoverse.CqrsKit.WebApi.DataAccess;
using Softoverse.CqrsKit.WebApi.Extensions;
using Softoverse.CqrsKit.WebApi.Middlewares;
using Softoverse.CqrsKit.WebApi.Models;
using Softoverse.CqrsKit.WebApi.Module;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
       .AddControllers(options =>
       {
           options.ModelValidatorProviders.Clear(); // Disable default asp.net core model state validation
       });

builder.Services.AddScoped<DocumentationAuthorizeMiddleware>();

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

app.UseMiddleware<DocumentationAuthorizeMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
    app.MapScalar();
}

app.UseStaticFiles();

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