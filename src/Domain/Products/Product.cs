using CraftersCloud.ReferenceArchitecture.Core;
using CraftersCloud.ReferenceArchitecture.Domain.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;
using CraftersCloud.ReferenceArchitecture.Domain.Products.DomainEvents;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

public class Product : EntityWithCreatedUpdated<ProductId>
{
    public const int NameMaxLength = 200;
    public const int DescriptionMaxLength = 2000;

    private Product() { }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ProductStatusId ProductStatusId { get; private set; } = null!;
    public ProductStatus ProductStatus { get; private set; } = null!;

    public static Product Create(CreateProductCommand command)
    {
        var result = new Product
        {
            Id = IStronglyTypedId<ProductId>.CreateNew(),
            Name = command.Name, Description = command.Description,
            ProductStatusId = command.ProductStatusId
        };

        result.AddDomainEvent(new ProductCreatedDomainEvent(result.Id, result.Name));
        return result;
    }

    public void Update(UpdateProductCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        ProductStatusId = command.ProductStatusId;
        AddDomainEvent(new ProductUpdatedDomainEvent(Id, Name));
    }
}