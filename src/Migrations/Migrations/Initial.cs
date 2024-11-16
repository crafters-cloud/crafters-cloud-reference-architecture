#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Permission",
            table => new
            {
                Id = table.Column<int>("int", nullable: false),
                Name = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permission", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "Role",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Role", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "UserStatus",
            table => new
            {
                Id = table.Column<int>("int", nullable: false),
                Description = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: false),
                Name = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserStatus", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "RolePermission",
            table => new
            {
                RoleId = table.Column<Guid>("uniqueidentifier", nullable: false),
                PermissionId = table.Column<int>("int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermission", x => new { x.PermissionId, x.RoleId });
                table.ForeignKey(
                    "FK_RolePermission_Permission_PermissionId",
                    x => x.PermissionId,
                    "Permission",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_RolePermission_Role_RoleId",
                    x => x.RoleId,
                    "Role",
                    "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "User",
            table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                EmailAddress = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
                FullName = table.Column<string>("nvarchar(200)", maxLength: 200, nullable: false),
                RoleId = table.Column<Guid>("uniqueidentifier", nullable: false),
                UserStatusId = table.Column<int>("int", nullable: false),
                CreatedById = table.Column<Guid>("uniqueidentifier", nullable: false),
                UpdatedById = table.Column<Guid>("uniqueidentifier", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>("datetimeoffset", nullable: false),
                UpdatedOn = table.Column<DateTimeOffset>("datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
                table.ForeignKey(
                    "FK_User_Role_RoleId",
                    x => x.RoleId,
                    "Role",
                    "Id");
                table.ForeignKey(
                    "FK_User_UserStatus_UserStatusId",
                    x => x.UserStatusId,
                    "UserStatus",
                    "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    "FK_User_User_CreatedById",
                    x => x.CreatedById,
                    "User",
                    "Id");
                table.ForeignKey(
                    "FK_User_User_UpdatedById",
                    x => x.UpdatedById,
                    "User",
                    "Id");
            });

        migrationBuilder.InsertData(
            "Permission",
            new[] { "Id", "Name" },
            new object[,]
            {
                { 1, "UsersRead" },
                { 2, "UsersWrite" }
            });

        migrationBuilder.InsertData(
            "Role",
            new[] { "Id", "Name" },
            new object[] { new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0"), "SystemAdmin" });

        migrationBuilder.InsertData(
            "UserStatus",
            new[] { "Id", "Description", "Name" },
            new object[,]
            {
                { 1, "Active Status Description", "Active" },
                { 2, "Inactive Status Description", "Inactive" }
            });

        migrationBuilder.InsertData(
            "RolePermission",
            new[] { "PermissionId", "RoleId" },
            new object[,]
            {
                { 1, new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0") },
                { 2, new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0") }
            });

        migrationBuilder.InsertData(
            "User",
            new[]
            {
                "Id", "CreatedById", "CreatedOn", "EmailAddress", "FullName", "RoleId", "UpdatedById", "UpdatedOn",
                "UserStatusId"
            },
            new object[]
            {
                new Guid("dfb44aa8-bfc9-4d95-8f45-ed6da241dcfc"), new Guid("dfb44aa8-bfc9-4d95-8f45-ed6da241dcfc"),
                new DateTimeOffset(new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)),
                "N/A", "System User", new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0"),
                new Guid("dfb44aa8-bfc9-4d95-8f45-ed6da241dcfc"),
                new DateTimeOffset(new DateTime(2024, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)),
                1
            });

        migrationBuilder.CreateIndex(
            "IX_Permission_Name",
            "Permission",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Role_Name",
            "Role",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_RolePermission_RoleId",
            "RolePermission",
            "RoleId");

        migrationBuilder.CreateIndex(
            "IX_User_CreatedById",
            "User",
            "CreatedById");

        migrationBuilder.CreateIndex(
            "IX_User_EmailAddress",
            "User",
            "EmailAddress",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_User_RoleId",
            "User",
            "RoleId");

        migrationBuilder.CreateIndex(
            "IX_User_UpdatedById",
            "User",
            "UpdatedById");

        migrationBuilder.CreateIndex(
            "IX_User_UserStatusId",
            "User",
            "UserStatusId");
    }
}