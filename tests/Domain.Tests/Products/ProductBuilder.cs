using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

namespace CraftersCloud.ReferenceArchitecture.Domain.Tests.Products;

public class ProductBuilder
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private ProductStatusId _statusId = ProductStatusId.Active;

    public ProductBuilder WithName(string value)
    {
        _name = value;
        return this;
    }

    public ProductBuilder WithDescription(string value)
    {
        _description = value;
        return this;
    }

    public ProductBuilder WithStatusId(ProductStatusId value)
    {
        _statusId = value;
        return this;
    }

    public static implicit operator Product(ProductBuilder builder) => ToProduct(builder);

    public static Product ToProduct(ProductBuilder builder) => builder.Build();

    public Product Build()
    {
        var result = Product.Create(new CreateProductCommand
        {
            Name = _name,
            Description = _description,
            ProductStatusId = _statusId
        });

        return result;
    }
}