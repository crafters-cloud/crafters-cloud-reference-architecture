using CraftersCloud.Core.SmartEnums.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly]
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Id).HasStronglyTypedId(x => x.Value, x => new ProductId(x));
        builder.Property(x => x.Name).IsRequired().HasMaxLength(Product.NameMaxLength);
        builder.Property(x => x.Description).HasMaxLength(Product.DescriptionMaxLength);

        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasReferenceTableRelationWithEnumAsForeignKey(x => x.ProductStatus, x => x.ProductStatusId);

        builder.HasCreatedByAndUpdatedBy();
    }
}