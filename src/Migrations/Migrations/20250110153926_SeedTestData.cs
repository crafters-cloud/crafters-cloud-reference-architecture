using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Migrations
{
    public partial class SeedTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ResourceSql("SeedTestData.sql");
        }
    }
}
