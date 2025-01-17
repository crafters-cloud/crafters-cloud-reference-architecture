using CraftersCloud.Core.SmartEnums.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ProductStatusConfiguration : IEntityTypeConfiguration<ProductStatus>
{
    public void Configure(EntityTypeBuilder<ProductStatus> builder)
    {
        builder.Property(x => x.Id).HasColumnType("tinyint");
        builder.ConfigureEntityWithEnumId<ProductStatus, ProductStatusId>(ProductStatus.NameMaxLength);
    }
}