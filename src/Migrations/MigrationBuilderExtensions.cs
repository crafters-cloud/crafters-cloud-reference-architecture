using CraftersCloud.Core.Helpers;
using CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations;

public static class MigrationBuilderExtensions
{
    public static void ResourceSql(this MigrationBuilder migrationBuilder, string scriptName)
    {
        var sql = EmbeddedResource.ReadResourceContent(typeof(DatabaseSeeding).Assembly,
            $"CraftersCloud.ReferenceArchitecture.Data.Migrations.Scripts.{scriptName}");
        if (!sql.HasContent())
        {
            throw new InvalidOperationException($"Script could not be loaded: {scriptName}");
        }

        migrationBuilder.Sql(sql);
    }
}