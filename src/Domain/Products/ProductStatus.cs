using CraftersCloud.Core.SmartEnums.Entities;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ProductStatus : EntityWithEnumId<ProductStatusId>
{
    public const int NameMaxLength = 50;
    public string Description { get; private set; } = string.Empty;
};