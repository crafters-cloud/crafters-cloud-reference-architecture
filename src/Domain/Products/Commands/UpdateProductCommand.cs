using CraftersCloud.Core.Cqrs;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
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
        public Validator(IServiceScopeFactory scopeFactory)
        {
            RuleFor(x => x.Name).ValidateProductName(x => x.Id, scopeFactory);
            RuleFor(x => x.Description).ValidateProductDescription();
            RuleFor(x => x.ProductStatusId).ValidateProductStatusId();
        }
    }
}