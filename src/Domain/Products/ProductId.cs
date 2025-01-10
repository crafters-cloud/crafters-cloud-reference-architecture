using CraftersCloud.ReferenceArchitecture.Core;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

public readonly record struct ProductId(Guid Value) : IStronglyTypedId<ProductId>
{
    public static implicit operator Guid(ProductId id) => id.Value;

    [UsedImplicitly]
    public static bool TryParse(string value, out ProductId result) =>
        IStronglyTypedId<ProductId>.TryParse(value, out result);

    public override string ToString() => Value.ToString();
}