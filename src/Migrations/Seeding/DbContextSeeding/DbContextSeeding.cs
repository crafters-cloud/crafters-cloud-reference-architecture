using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

namespace CraftersCloud.ReferenceArchitecture.Migrations.Seeding.DbContextSeeding;

public static class DbContextSeeding
{
    private static readonly List<IDbContextSeeding<AppDbContext>> Seedings =
    [
        new ProductSeeding()
    ];

    public static void Seed(AppDbContext dbContext) => Seedings.ForEach(seeding => seeding.Seed(dbContext));
}