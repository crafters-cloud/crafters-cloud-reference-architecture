using CraftersCloud.Core.EntityFramework.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding;

public static class DatabaseSeeder
{
    private static readonly List<ISeeding> Seedings =
    [
        new UserStatusSeeding(),
        new RolePermissionSeeding(),
        new UserSeeding(),
        new ProductStatusSeeding(),
        new ProductSeeding()
    ];

    public static void Seed(ModelBuilder modelBuilder) => Seedings.ForEach(seeding => seeding.Seed(modelBuilder));
}