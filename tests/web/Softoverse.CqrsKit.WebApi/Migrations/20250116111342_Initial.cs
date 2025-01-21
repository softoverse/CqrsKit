using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Softoverse.CqrsKit.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalFlowConfigurations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalFlowConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalFlowPendingTasks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CompletedStepNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewerUsername = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalFlowDetailsPageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalFlowCorrectionPageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsCorrectionAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommandName = table.Column<string>(type: "TEXT", nullable: false),
                    CommandNamespace = table.Column<string>(type: "TEXT", nullable: false),
                    CommandFullName = table.Column<string>(type: "TEXT", nullable: false),
                    CommandId = table.Column<long>(type: "INTEGER", nullable: true),
                    ResponseName = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseNamespace = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseFullName = table.Column<string>(type: "TEXT", nullable: true),
                    HandlerName = table.Column<string>(type: "TEXT", nullable: true),
                    HandlerNamespace = table.Column<string>(type: "TEXT", nullable: true),
                    HandlerFullName = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalFlowHandlerName = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalFlowHandlerNamespace = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalFlowHandlerFullName = table.Column<string>(type: "TEXT", nullable: true),
                    Payload = table.Column<byte[]>(type: "BLOB", nullable: true),
                    UniqueIdentification = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewedBy = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalFlowPendingTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommandQueries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Namespace = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    ResponseName = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseNamespace = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseFullName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCommand = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsApprovalFlowRequired = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    AgeCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandApprovalFlowConfigurations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApprovalFlowConfigurationId = table.Column<long>(type: "INTEGER", nullable: false),
                    CommandId = table.Column<long>(type: "INTEGER", nullable: false),
                    ApprovalFlowConfigurationId1 = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandApprovalFlowConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandApprovalFlowConfigurations_ApprovalFlowConfigurations_ApprovalFlowConfigurationId1",
                        column: x => x.ApprovalFlowConfigurationId1,
                        principalTable: "ApprovalFlowConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandApprovalFlowConfigurations_CommandApprovalFlowConfigurations_ApprovalFlowConfigurationId",
                        column: x => x.ApprovalFlowConfigurationId,
                        principalTable: "CommandApprovalFlowConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandApprovalFlowConfigurations_CommandQueries_CommandId",
                        column: x => x.CommandId,
                        principalTable: "CommandQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalFlowConfigurationSteps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalFlowConfigurationId = table.Column<long>(type: "INTEGER", nullable: false),
                    UserGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsSelectReviewer = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCorrectionAllowed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalFlowConfigurationSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalFlowConfigurationSteps_ApprovalFlowConfigurations_ApprovalFlowConfigurationId",
                        column: x => x.ApprovalFlowConfigurationId,
                        principalTable: "ApprovalFlowConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalFlowConfigurationSteps_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandQueryUserGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    CommandQueryId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandQueryUserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandQueryUserGroups_CommandQueries_CommandQueryId",
                        column: x => x.CommandQueryId,
                        principalTable: "CommandQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandQueryUserGroups_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupUsers_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8c8ba378-85d1-4e11-8a6a-ea922c7bd44b", 0, "ba9a687d-f69c-47a5-8ba5-6ca5e92129ef", "user@gmail.com", false, false, null, "USER@GMAIL.COM", "USER", "AQAAAAIAAYagAAAAEP1mOA2ZPOYDCwE8/NchGK5wDsE76sPexTcKYryAw7nHeT1PUWlK8P7HQwMxR6Xlkg==", null, false, "9561b38e-379d-4dd3-99da-705cb970acc5", false, "user" },
                    { "94df2b00-1fed-4067-a2c4-dfac4c192135", 0, "0cc18343-0e52-4443-a563-a7a4def78282", "admin@gmail.com", false, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEAmMzPLYXKCkk2TPO9QqywIKAApEBxQ4sBJdqLLNeePZ0o3Ix30ObawpxOGW3/OrcQ==", null, false, "af5f60a2-9171-48bc-8ca3-5c4e64f5a354", false, "admin" },
                    { "eaca3107-de08-4d1f-93ff-ca3d87ea137e", 0, "f5036f71-0239-4b67-b775-5adbe5fcbc98", "superadmin@gmail.com", false, false, null, "SUPERADMIN@GMAIL.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEL++WXwo+LwK5uZdFmR5By0Y3eenLFum2Wc5wMIRlyJMJYsfyhkXaIvfRdTkaizy9Q==", null, false, "a7bc295d-abac-4c3b-9d33-833258c2716d", false, "superadmin" },
                    { "f5bb9bf9-10a8-4e32-8054-3dd9a9d320d7", 0, "0cc18343-0e52-4443-a563-a7a4def78282", "admin2@gmail.com", false, false, null, "ADMIN2@GMAIL.COM", "ADMIN2", "AQAAAAIAAYagAAAAEOFgFuyU/Tg0MYk+JuXydSf/FLhIxAGHN1+crUOGE6yUBK6Q81IDmSg5sg3G5gwglQ==", null, false, "af5f60a2-9171-48bc-8ca3-5c4e64f5a354", false, "admin2" }
                });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1L, null, "Super Admin" },
                    { 2L, null, "Admin" },
                    { 3L, null, "User" }
                });

            migrationBuilder.InsertData(
                table: "UserGroupUsers",
                columns: new[] { "Id", "UserGroupId", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, "eaca3107-de08-4d1f-93ff-ca3d87ea137e" },
                    { 2L, 2L, "94df2b00-1fed-4067-a2c4-dfac4c192135" },
                    { 3L, 2L, "f5bb9bf9-10a8-4e32-8054-3dd9a9d320d7" },
                    { 4L, 3L, "8c8ba378-85d1-4e11-8a6a-ea922c7bd44b" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalFlowConfigurationSteps_ApprovalFlowConfigurationId",
                table: "ApprovalFlowConfigurationSteps",
                column: "ApprovalFlowConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalFlowConfigurationSteps_UserGroupId",
                table: "ApprovalFlowConfigurationSteps",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandApprovalFlowConfigurations_ApprovalFlowConfigurationId",
                table: "CommandApprovalFlowConfigurations",
                column: "ApprovalFlowConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandApprovalFlowConfigurations_ApprovalFlowConfigurationId1",
                table: "CommandApprovalFlowConfigurations",
                column: "ApprovalFlowConfigurationId1");

            migrationBuilder.CreateIndex(
                name: "IX_CommandApprovalFlowConfigurations_CommandId",
                table: "CommandApprovalFlowConfigurations",
                column: "CommandId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandQueries_Name_Namespace_FullName_ResponseName_ResponseNamespace_ResponseFullName",
                table: "CommandQueries",
                columns: new[] { "Name", "Namespace", "FullName", "ResponseName", "ResponseNamespace", "ResponseFullName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandQueryUserGroups_CommandQueryId",
                table: "CommandQueryUserGroups",
                column: "CommandQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandQueryUserGroups_UserGroupId_CommandQueryId",
                table: "CommandQueryUserGroups",
                columns: new[] { "UserGroupId", "CommandQueryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupUsers_UserGroupId",
                table: "UserGroupUsers",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupUsers_UserId",
                table: "UserGroupUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalFlowConfigurationSteps");

            migrationBuilder.DropTable(
                name: "ApprovalFlowPendingTasks");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CommandApprovalFlowConfigurations");

            migrationBuilder.DropTable(
                name: "CommandQueryUserGroups");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "UserGroupUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ApprovalFlowConfigurations");

            migrationBuilder.DropTable(
                name: "CommandQueries");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
