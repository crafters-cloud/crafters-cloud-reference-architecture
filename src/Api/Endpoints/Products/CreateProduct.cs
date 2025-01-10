using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static partial class CreateProduct
{
    public sealed record Request(string Name, string Description, ProductStatusId ProductStatusId);

    public static async Task<Results<Created<Product>, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = UpdateProductRequestMapper.ToCommand(request);
        var commandResult = await sender.Send(command, cancellationToken);
        var results = commandResult.ToMinimalApiResult(httpContext, product => $"/products/{product.Id}");
        return results;
    }

    [Mapper]
    public static partial class UpdateProductRequestMapper
    {
        public static partial CreateProductCommand ToCommand(Request source);
    }
}