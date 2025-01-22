using CraftersCloud.Core.Cqrs;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

[PublicAPI]
public record CreateProductCommand(string Name, string Description, ProductStatusId ProductStatusId) : ICommand<CreateCommandResult<Product>>
{
    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateProductCommand>
    {
        public Validator(IServiceScopeFactory scopeFactory)
        {
            RuleFor(x => x.Name).ValidateProductName(x => null, scopeFactory);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }
    }
}