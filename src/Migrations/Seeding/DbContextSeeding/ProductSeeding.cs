using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding.DbContextSeeding;

internal class ProductSeeding : IDbContextSeeding<AppDbContext>
{
    public void Seed(AppDbContext dbContext)
    {
        var id = ProductId.Create(new Guid("9F39D68D-83E6-4481-871F-F809A3EBA998"));
        var product = dbContext.Set<Product>().QueryById(id).SingleOrDefault();
        if (product == null)
        {
            var asOf = new DateTimeOffset(2025, 1, 10, 0, 0, 0, TimeSpan.Zero);
            product = Product.Create(new CreateProductCommand("Software development", "Lorem ipsum",
                ProductStatusId.Active));

            product.SetCreated(asOf, User.SystemUserId);
            product.SetUpdated(asOf, User.SystemUserId);

            dbContext.Set<Product>().Add(product);
        }
    }
}