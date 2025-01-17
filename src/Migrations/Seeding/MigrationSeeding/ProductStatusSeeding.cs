using CraftersCloud.Core.SmartEnums.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Products;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding.MigrationSeeding;

internal class ProductStatusSeeding() : EntityWithEnumIdSeeding<ProductStatus, ProductStatusId>(statusId =>
    new { Id = statusId, statusId.Name, Description = GetDescription(statusId) })
{
    private static string GetDescription(ProductStatusId statusId) =>
        statusId.Value == ProductStatusId.Active
            ? "Active Status Description"
            : statusId.Value == ProductStatusId.Inactive
                ? "Inactive Status Description"
                : string.Empty;
}