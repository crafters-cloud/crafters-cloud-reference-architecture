using CraftersCloud.Core.Helpers;
using CraftersCloud.ReferenceArchitecture.Migrations.Seeding.MigrationSeeding;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftersCloud.ReferenceArchitecture.Migrations;

public static class MigrationBuilderExtensions
{
    public static void ResourceSql(this MigrationBuilder migrationBuilder, string scriptName)
    {
        var sql = EmbeddedResource.ReadResourceContent(typeof(MigrationSeeding).Assembly,
            $"CraftersCloud.ReferenceArchitecture.Migrations.Scripts.{scriptName}");
        if (!sql.HasContent())
        {
            throw new InvalidOperationException($"Script could not be loaded: {scriptName}");
        }

        migrationBuilder.Sql(sql);
    }
}