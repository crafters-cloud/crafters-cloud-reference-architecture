using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

[UsedImplicitly]
public class ProductEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("products")
            .WithGroupName("Products")
            .RequireAuthorization()
            .RequirePermissions(PermissionId.ProductsRead);

        group.MapGet("/", GetProducts.Handle).Validate<GetProducts.Request>();
        group.MapGet("/{id:guid}", GetProductById.Handle);

        group.MapPut("/", UpdateProduct.Handle).Validate<UpdateProduct.Request>()
            .RequirePermissions(PermissionId.ProductsWrite);
        group.MapPost("/", CreateProduct.Handle).Validate<CreateProduct.Request>()
            .RequirePermissions(PermissionId.ProductsWrite);

        group.MapGet("/statuses", GetProductStatuses.Handle);
    }
}