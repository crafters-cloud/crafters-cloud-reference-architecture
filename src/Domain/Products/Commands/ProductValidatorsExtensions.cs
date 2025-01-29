using CraftersCloud.Core;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

public static class ProductValidatorsExtensions
{
    public delegate ProductId? GetId<in T>(T source);

    public static void ValidateProductName<T>(this IRuleBuilder<T, string> ruleBuilder, GetId<T> getId,
        IServiceScopeFactory factory)
    {
        ruleBuilder.NotEmpty().MaximumLength(Product.NameMaxLength);
        ruleBuilder.MustAsync((command, name, cancellationToken) =>
                IsProductNameUnique(getId(command), name, factory, cancellationToken))
            .WithMessage("Product name is already taken");
    }

    private static async Task<bool> IsProductNameUnique(ProductId? id, string name, IServiceScopeFactory factory,
        CancellationToken cancellationToken)
    {
        using var scope = factory.CreateScope();
        var repository = scope.Resolve<IRepository<Product>>();
        return !await repository.QueryAll()
            .QueryExceptWithIdOptional(id)
            .QueryByName(name)
            .AnyAsync(cancellationToken);
    }

    public static void ValidateProductDescription<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.MaximumLength(Product.DescriptionMaxLength);

    public static void ValidateProductStatusId<T>(this IRuleBuilder<T, ProductStatusId> ruleBuilder) =>
        ruleBuilder.NotEmpty();
}