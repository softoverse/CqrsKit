using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Softoverse.CqrsKit.Models.Entity;
using Softoverse.CqrsKit.Services;
using Softoverse.CqrsKit.WebApi.Models.CQRS;

namespace Softoverse.CqrsKit.WebApi.DataAccess;

public static class DatabaseSeeder
{
    public static async Task SeedApplicationBaseDataAsync(this WebApplication? app)
    {
        if (app == null)
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        try
        {
            //Auto migrate if database exists
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            if (await dbContext.Database.CanConnectAsync())
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    //Migrate Database as the database is already there
                    await dbContext.Database.MigrateAsync();
                }
            }
            else
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    //First Migrate then ensure Created to avoid database errors
                    await dbContext.Database.MigrateAsync();

                    //Ensures that Database is created
                    await dbContext.Database.EnsureCreatedAsync();
                }
            }

            services.SeedCqrsData();
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<IWebApiDataAccessAssemblyMarker>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }

    private static void SeedCqrsData(this IServiceProvider services)
    {
        ApplicationDbContext dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.SeedCqrsCommandQueries();
    }

    /// <summary>
    /// Use this method if you want only to save queries into the database
    /// </summary>
    /// <param name="dbContext"></param>
    private static void SeedCqrsCommandQueries(this ApplicationDbContext dbContext)
    {
        try
        {
            IEnumerable<BaseCommandQuery> commandQueries = CqrsHelper.GetAllCommandQueryTypes();
            IEnumerable<CommandQuery> commandQueriesFromDb = dbContext.CommandQueries;
            List<CommandQuery> commandQueriesToInsert = new List<CommandQuery>();

            foreach (var baseCommandQuery in commandQueries)
            {
                bool hasInDb = commandQueriesFromDb.Any(commandQuery => GetCommandQueryCriteria(commandQuery, baseCommandQuery) && GetCommandQueryResponseCriteria(commandQuery, baseCommandQuery));

                if (!hasInDb)
                {
                    CommandQuery command = CqrsHelper.ToChildOfBaseCommandQuery<CommandQuery>(baseCommandQuery);
                    // Set extra values property of a command object if needed
                    commandQueriesToInsert.Add(command);
                }
            }

            if (commandQueriesToInsert.Count > 0)
            {
                dbContext.CommandQueries.AddRangeAsync(commandQueriesToInsert);
                dbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not save command queries");
            Console.WriteLine(ex.Message);
        }
    }

    private static readonly Func<CommandQuery, BaseCommandQuery, bool> GetCommandQueryCriteria = (commandQuery, baseCommandQuery) =>
        commandQuery.Name == baseCommandQuery.Name
      &&
        commandQuery.Namespace == baseCommandQuery.Namespace
      &&
        commandQuery.FullName == baseCommandQuery.FullName;

    private static readonly Func<CommandQuery, BaseCommandQuery, bool> GetCommandQueryResponseCriteria = (commandQuery, baseCommandQuery) =>
        commandQuery.ResponseName == baseCommandQuery.ResponseName
      &&
        commandQuery.ResponseNamespace == baseCommandQuery.ResponseNamespace
      &&
        commandQuery.ResponseFullName == baseCommandQuery.ResponseFullName;
}

/*
*  => For Mapping CommandQuery with User Group In Start
*  => API: baseUrl/api/CommandQueryUserGroups/range
*  => Body:
[
 {
   "id": 0,
   "userGroupId": 1,
   "commandQueryId": 1
 },
 {
   "id": 0,
   "userGroupId": 2,
   "commandQueryId": 2
 },
 {
   "id": 0,
   "userGroupId": 1,
   "commandQueryId": 3
 },
 {
   "id": 0,
   "userGroupId": 3,
   "commandQueryId": 4
 },
 {
   "id": 0,
   "userGroupId": 3,
   "commandQueryId": 5
 }
]
*/