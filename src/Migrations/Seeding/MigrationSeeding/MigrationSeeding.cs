using CraftersCloud.Core.EntityFramework.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Migrations.Seeding.MigrationSeeding;

public static class MigrationSeeding
{
    private static readonly List<IModelBuilderSeeding> Seedings =
    [
        new UserStatusSeeding(),
        new RolePermissionSeeding(),
        new UserSeeding(),
        new ProductStatusSeeding()
    ];

    public static void Seed(ModelBuilder modelBuilder) => Seedings.ForEach(seeding => seeding.Seed(modelBuilder));
}