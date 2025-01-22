using CraftersCloud.ReferenceArchitecture.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ResourceSql("SeedTestData.sql");
        }
    }
}
