using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding;

internal class ProductSeeding : ISeeding
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var asOf = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero);
        var product = new
        {
            Id = Guid.Parse("9F39D68D-83E6-4481-871F-F809A3EBA998"),
            Name = "Software development",
            Description = "Lorem ipsum",
            CreatedById = User.SystemUserId,
            CreatedOn = asOf,
            UpdatedById = User.SystemUserId,
            UpdatedOn = asOf,
            ProductStatusId = ProductStatusId.Active
        };
        modelBuilder.Entity<Product>().HasData(product);
    }
}