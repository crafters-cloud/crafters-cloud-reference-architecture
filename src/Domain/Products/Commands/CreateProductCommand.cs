using CraftersCloud.Core;
using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

[PublicAPI]
public record CreateProductCommand(string Name, string Description, ProductStatusId ProductStatusId) : ICommand<CreateCommandResult<Product>>
{
    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateProductCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.Name).ValidateProductName(UniqueProductName);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }

        private async Task<bool> UniqueProductName(CreateProductCommand command, string name, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<Product>>();
            return !await repository.QueryAll()
                .QueryByName(name)
                .AnyAsync(cancellationToken);
        }
    }
}