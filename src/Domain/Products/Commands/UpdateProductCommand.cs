using CraftersCloud.Core;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Messaging;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

[PublicAPI]
public class UpdateProductCommand : ICommand<UpdateCommandResult<Product>>
{
    public ProductId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProductStatusId ProductStatusId { get; set; } = ProductStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<UpdateProductCommand>
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

        private async Task<bool> UniqueProductName(UpdateProductCommand command, string name,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<Product, ProductId>>();
            return !await repository.QueryAll()
                .QueryExceptWithId(command.Id)
                .QueryByName(name)
                .AnyAsync(cancellationToken);
        }
    }
}