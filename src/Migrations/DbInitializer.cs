using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations;

public static class DbInitializer
{
    private static readonly List<ISeeding> Seeding =
    [
        new UserStatusSeeding(),
        new RolePermissionSeeding(),
        new UserSeeding()
    ];

    public static void SeedData(ModelBuilder modelBuilder) => Seeding.ForEach(seeding => seeding.Seed(modelBuilder));
}