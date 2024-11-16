#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Migrations;

public partial class SeedData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.ResourceSql("SeedTestData.sql");
}