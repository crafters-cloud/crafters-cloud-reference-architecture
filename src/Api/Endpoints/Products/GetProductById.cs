using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Products;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static partial class GetProductById
{
    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public ProductStatusId ProductStatusId { get; set; } = ProductStatusId.Active;
        public string ProductStatusName { get; set; } = string.Empty;
        public string ProductStatusDescription { get; set; } = string.Empty;
    }

    [Mapper]
    public static partial class Mapper
    {
        public static partial Response Map(Product source);
    }

    public static async Task<Results<Ok<Response>, NotFound>> Handle(ProductId id,
        IRepository<Product> repository,
        CancellationToken cancellationToken)
    {
        var entity = await repository.QueryAll()
            .Include(x => x.ProductStatus)
            .AsNoTracking()
            .QueryById(id)
            .SingleOrDefaultAsync(cancellationToken);

        return entity.ToMinimalApiResult(Mapper.Map);
    }
}