using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Softoverse.CqrsKit.WebApi.DataAccess;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class DatabaseConfigurationExtension
{
    public static WebApplicationBuilder AddDatabaseConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

        string? connectionString = connectionString = builder.Configuration.GetConnectionString("SqLiteDatabase");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        
        return builder;
    }
}