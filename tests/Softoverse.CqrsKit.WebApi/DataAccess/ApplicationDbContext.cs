using System.Data;
using System.Data.Common;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Softoverse.CqrsKit.WebApi.Models;

namespace Softoverse.CqrsKit.WebApi.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    private DbSet<Student> Students { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // #region Fluent API
        //
        // builder.Entity<CommandQuery>()
        //     .HasIndex(x => new { x.Name, x.Namespace, x.FullName, x.ResponseName, x.ResponseNamespace, x.ResponseFullName }).IsUnique();
        //
        // #region CommandQueryUserGroup
        //
        // builder.Entity<CommandQueryUserGroup>()
        //     .HasOne(cqu => cqu.CommandQuery)
        //     .WithMany(cq => cq.CommandQueryUserGroups)
        //     .HasForeignKey(cqu => cqu.CommandQueryId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.Entity<CommandQueryUserGroup>()
        //     .HasOne(cqu => cqu.UserGroup)
        //     .WithMany(ug => ug.CommandQueryUserGroups)
        //     .HasForeignKey(cqu => cqu.UserGroupId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.Entity<CommandQueryUserGroup>().HasIndex(h => new { h.UserGroupId, h.CommandQueryId }).IsUnique();
        //
        // #endregion CommandQueryUserGroup
        //
        // builder.Entity<ApprovalFlowConfigurationDetail>()
        //     .HasOne(afcd => afcd.ApprovalFlowConfiguration)
        //     .WithMany(afc => afc.ApprovalFlowConfigurationDetails)
        //     .HasForeignKey(afcd => afcd.ApprovalFlowConfigurationId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // #region CommandApprovalFlowConfiguration { One(CommandQuery) to many(ApprovalFlowConfiguration) relationship with mapping table }
        //
        // builder.Entity<CommandApprovalFlowConfiguration>()
        //     .HasOne(cafc => cafc.Command)
        //     .WithOne(cq => cq.CommandApprovalFlowConfiguration)
        //     .HasForeignKey<CommandApprovalFlowConfiguration>(cafc => cafc.CommandId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.Entity<CommandApprovalFlowConfiguration>()
        //     .HasOne(cafc => cafc.ApprovalFlowConfiguration)
        //     .WithMany(afc => afc.CommandApprovalFlowConfigurations)
        //     .HasForeignKey(cafc => cafc.ApprovalFlowConfigurationId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.Entity<CommandQuery>()
        //     .HasOne(cafc => cafc.CommandApprovalFlowConfiguration)
        //     .WithOne(afc => afc.Command);
        //
        // builder.Entity<CommandApprovalFlowConfiguration>().HasIndex(h => new { h.CommandId }).IsUnique();
        //
        // #endregion CommandApprovalFlowConfiguration { One(CommandQuery) to many(ApprovalFlowConfiguration) relationship with mapping table }
        //
        // #endregion Fluent API
        //
        // #region Data seed
        //
        // IdentityUser[] users = new IdentityUser[]
        // {
        //     new()
        //     {
        //         Id = "eaca3107-de08-4d1f-93ff-ca3d87ea137e",
        //         ConcurrencyStamp = "f5036f71-0239-4b67-b775-5adbe5fcbc98",
        //         SecurityStamp = "a7bc295d-abac-4c3b-9d33-833258c2716d",
        //         UserName = "superadmin",
        //         NormalizedUserName = "SUPERADMIN",
        //         Email = "superadmin@gmail.com",
        //         NormalizedEmail = "SUPERADMIN@GMAIL.COM",
        //         PasswordHash = "AQAAAAIAAYagAAAAEL5+EHnuqwPGWG4NQw9IhPidoFfR51BTlRIYA1wceV9H50ewyBse+4dvrlI5sANsew=="
        //     },
        //     new()
        //     {
        //         Id = "94df2b00-1fed-4067-a2c4-dfac4c192135",
        //         ConcurrencyStamp = "0cc18343-0e52-4443-a563-a7a4def78282",
        //         SecurityStamp = "af5f60a2-9171-48bc-8ca3-5c4e64f5a354",
        //         UserName = "admin",
        //         NormalizedUserName = "ADMIN",
        //         Email = "admin@gmail.com",
        //         NormalizedEmail = "ADMIN@GMAIL.COM",
        //         PasswordHash = "AQAAAAIAAYagAAAAEMqnN+XJyX5WIe5xXjSPDN8IPpT115UUvriaVh86W4rkAQfihp1iLaY0DnUeXftQHQ=="
        //     },
        //     new()
        //     {
        //         Id = "8c8ba378-85d1-4e11-8a6a-ea922c7bd44b",
        //         ConcurrencyStamp = "ba9a687d-f69c-47a5-8ba5-6ca5e92129ef",
        //         SecurityStamp = "9561b38e-379d-4dd3-99da-705cb970acc5",
        //         UserName = "user",
        //         NormalizedUserName = "USER",
        //         Email = "user@gmail.com",
        //         NormalizedEmail = "USER@GMAIL.COM",
        //         PasswordHash = "AQAAAAIAAYagAAAAEFB5UG7+PWFT80tsImaOaFReHJKM7xv0/ogl3SrhkJZyjU5MQaNzht4YklkE2MAmMw=="
        //     },
        // };
        //
        // //var passwordHasher = new PasswordHasher<IdentityUser>();
        // //foreach (var user in users)
        // //{
        // //    user.PasswordHash = passwordHasher.HashPassword(user, user.UserName + "@12");
        // //}
        //
        // UserGroup[] userGroups = new UserGroup[]
        // {
        //     new(){ Id = 1, Name = "Super Admin" },
        //     new(){ Id = 2, Name = "Admin" },
        //     new(){ Id = 3, Name = "User" },
        // };
        //
        // UserGroupUser[] userGroupUsers = new UserGroupUser[]
        // {
        //     new(){ Id = 1, UserGroupId = userGroups[0].Id, UserId = users[0].Id },
        //     new(){ Id = 2, UserGroupId = userGroups[1].Id, UserId = users[1].Id },
        //     new(){ Id = 3, UserGroupId = userGroups[2].Id, UserId = users[2].Id },
        // };
        //
        // builder.Entity<IdentityUser>().HasData(users);
        // builder.Entity<UserGroup>().HasData(userGroups);
        // builder.Entity<UserGroupUser>().HasData(userGroupUsers);
        //
        // #endregion Data seed

    }


    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<IDbContextTransaction, Task<TResult>> operation, CancellationToken ct = default) where TResult : class, new()
    {
        IExecutionStrategy strategy = Database.CreateExecutionStrategy();
        TResult response = await strategy.ExecuteAsync(async () =>
        {
            using IDbContextTransaction transaction = await Database.BeginTransactionAsync(ct);
            try
            {
                var response = await operation(transaction);

                DbTransaction? dbTransaction = transaction.GetDbTransaction();

                if (dbTransaction != null! && !IsTransactionCommitted())
                {
                    await transaction.CommitAsync(ct);
                }
                return response;
            }
            catch (Exception ex)
            {
                DbTransaction? dbTransaction = transaction.GetDbTransaction();

                if (dbTransaction != null! && !IsTransactionCommitted())
                {
                    await transaction.RollbackAsync(ct);
                }

                throw;
            }
        });
        return response;
    }

    public Task ExecuteInTransactionAsync(Func<IDbContextTransaction, Task> operation, CancellationToken ct = default)
    {
        IExecutionStrategy strategy = Database.CreateExecutionStrategy();
        var task = strategy.ExecuteAsync(async () =>
        {
            await using IDbContextTransaction transaction = await Database.BeginTransactionAsync(ct);
            try
            {
                await operation(transaction);

                DbTransaction? dbTransaction = transaction.GetDbTransaction();

                if (dbTransaction != null! && !IsTransactionCommitted())
                {
                    await transaction.CommitAsync(ct);
                }
            }
            catch (Exception ex)
            {
                DbTransaction? dbTransaction = transaction.GetDbTransaction();

                if (dbTransaction != null! && !IsTransactionCommitted())
                {
                    await transaction.RollbackAsync(ct);
                }

                throw;
            }
        });

        return task;
    }

    private bool IsTransactionCommitted()
    {
        var connection = Database.GetDbConnection();
        var isCommitted = connection.State == ConnectionState.Closed || Database.CurrentTransaction == null;

        return isCommitted;
    }
}