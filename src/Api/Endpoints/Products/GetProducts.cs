using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Products;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static partial class GetProducts
{
    [PublicAPI]
    public class Request : PagedQuery<Response.Item>
    {
        [FromQuery] public string? Name { get; set; }
        [FromQuery] public ProductStatusId? ProductStatusId { get; set; }
    }

    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            this.AddPagedQueryRules();
            RuleFor(x => x.Name).MaximumLength(100);
        }
    }

    [PublicAPI]
    public class Response
    {
        [PublicAPI]
        public class Item
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string ProductStatusName { get; set; } = string.Empty;
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
        }
    }

    [Mapper]
    public static partial class ResponseItemQueryMapper
    {
        public static partial IQueryable<Response.Item> ProjectTo(IQueryable<Product> q);
    }

    public static async Task<Ok<PagedQueryResponse<Response.Item>>> Handle([AsParameters] Request request,
        IRepository<Product, ProductId> repository,
        CancellationToken cancellationToken)
    {
        var query = repository.QueryAll()
            .Include(u => u.ProductStatus)
            .AsNoTracking()
            .QueryByStatusOptional(request.ProductStatusId)
            .QueryByNameOptional(request.Name);

        var items = await ResponseItemQueryMapper.ProjectTo(query)
            .ToPagedResponseAsync(request, cancellationToken);

        return TypedResults.Ok(items);
    }
}