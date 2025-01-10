using CraftersCloud.Core.SmartEnums.Entities;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ProductStatus : EntityWithEnumId<ProductStatusId>
{
    public string Description { get; private set; } = string.Empty;
};