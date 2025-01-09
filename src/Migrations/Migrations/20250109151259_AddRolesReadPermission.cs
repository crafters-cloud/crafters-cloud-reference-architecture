using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesReadPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "RolesRead" });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { 3, new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0") });

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
