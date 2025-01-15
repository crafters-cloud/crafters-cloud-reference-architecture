using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using FluentValidation;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public static class UserValidatorsExtensions
{
    internal static void ValidateUserEmail<T>(this IRuleBuilder<T, string> ruleBuilder,
        Func<T, string, CancellationToken, Task<bool>> predicate)
    {
        ruleBuilder.NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
        ruleBuilder.MustAsync(predicate).WithMessage("EmailAddress is already taken");
    }

    internal static void ValidateUserFirstName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().MaximumLength(User.FirstNameMaxLength);

    internal static void ValidateUserLastName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().MaximumLength(User.LastNameMaxLength);

    internal static void ValidateRoleId<T>(this IRuleBuilder<T, RoleId> ruleBuilder) =>
        ruleBuilder.NotEmpty();
}