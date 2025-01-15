using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using FluentValidation;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public static class UserValidatorsExtensions
{
    public static void ValidateUserEmail<T>(this IRuleBuilder<T, string> ruleBuilder,
        Func<T, string, CancellationToken, Task<bool>> predicate)
    {
        ruleBuilder.NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
        ruleBuilder.MustAsync(predicate).WithMessage("EmailAddress is already taken");
    }

    public static void ValidateUserFirstName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().MaximumLength(User.FirstNameMaxLength);

    public static void ValidateUserLastName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().MaximumLength(User.LastNameMaxLength);

    public static void ValidateRoleId<T>(this IRuleBuilder<T, RoleId> ruleBuilder) =>
        ruleBuilder.NotEmpty();
    
    public static void ValidateRoleId<T>(this IRuleBuilder<T, Guid> ruleBuilder) =>
        ruleBuilder.NotEmpty();
}