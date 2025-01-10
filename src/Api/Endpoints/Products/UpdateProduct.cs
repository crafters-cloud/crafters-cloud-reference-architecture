using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static partial class UpdateProduct
{
    public sealed record Request(
        Guid Id,
        string Name,
        string Description,
        ProductStatusId ProductStatusId);

    public static async Task<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var command = UpdateProductRequestMapper.ToCommand(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult(context);
    }

    [Mapper]
    public static partial class UpdateProductRequestMapper
    {
        public static partial UpdateProductCommand ToCommand(Request source);
    }
}