using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;
using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static partial class UpdateProduct
{
    public sealed record Request(
        ProductId Id,
        string Name,
        string Description,
        ProductStatusId ProductStatusId);

    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        public Validator(IServiceScopeFactory scopeFactory)
        {
            RuleFor(x => x.Name).ValidateProductName(x => x.Id, scopeFactory);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }
    }

    public static async Task<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult(context);
    }

    [Mapper]
    public static partial class Mapper
    {
        public static partial UpdateProductCommand Map(Request source);
    }
}