using CraftersCloud.Core;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Cqrs;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

[PublicAPI]
public class CreateProductCommand : ICommand<CreateCommandResult<Product>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProductStatusId ProductStatusId { get; set; } = ProductStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateProductCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.Name).NotEmpty().MaximumLength(Product.NameMaxLength);
            RuleFor(x => x.Name).MustAsync(UniqueProductName).WithMessage("Product name is already taken");
            RuleFor(x => x.Description).MaximumLength(Product.DescriptionMaxLength);
            RuleFor(x => x.ProductStatusId).NotEmpty();
        }

        private async Task<bool> UniqueProductName(CreateProductCommand command, string name,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<Product>>();
            return !await repository.QueryAll()
                .QueryByName(name)
                .AnyAsync(cancellationToken);
        }
    }
}