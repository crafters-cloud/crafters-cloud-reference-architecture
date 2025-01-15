using FluentValidation;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.Commands;

public static class ProductValidatorsExtensions
{
    public static void ValidateProductName<T>(this IRuleBuilder<T, string> ruleBuilder,
        Func<T, string, CancellationToken, Task<bool>> predicate)
    {
        ruleBuilder.NotEmpty().MaximumLength(Product.NameMaxLength);
        ruleBuilder.MustAsync(predicate).WithMessage("Product name is already taken");
    }

    public static void ValidateProductDescription<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.MaximumLength(Product.DescriptionMaxLength);

    public static void ValidateProductStatusId<T>(this IRuleBuilder<T, ProductStatusId> ruleBuilder) =>
        ruleBuilder.NotEmpty();
}