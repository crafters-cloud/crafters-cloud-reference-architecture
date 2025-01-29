using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static partial class CreateProduct
{
    public sealed record Request(string Name, string Description, ProductStatusId ProductStatusId);

    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        public Validator(IServiceScopeFactory scopeFactory)
        {
            RuleFor(x => x.Name).ValidateProductName(x => null, scopeFactory);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }
    }

    [Mapper]
    public static partial class Mapper
    {
        public static partial CreateProductCommand Map(Request source);
    }

    public static async Task<Results<Created<Product>, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult(httpContext, product => $"/products/{product.Id}");
    }
}