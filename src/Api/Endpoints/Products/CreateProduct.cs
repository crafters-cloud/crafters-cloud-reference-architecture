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
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.Name).ValidateProductName(UniqueProductName);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }

        private async Task<bool> UniqueProductName(Request command, string name,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<Product>>();
            return !await repository.QueryAll()
                .QueryByName(name)
                .AnyAsync(cancellationToken);
        }
    }

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