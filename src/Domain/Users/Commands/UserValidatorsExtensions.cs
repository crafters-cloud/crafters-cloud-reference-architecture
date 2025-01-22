using CraftersCloud.Core;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public static class UserValidatorsExtensions
{
    public delegate UserId? GetId<in T>(T source);

    public static void ValidateUserEmail<T>(this IRuleBuilder<T, string> ruleBuilder,
        GetId<T> getId,
        IServiceScopeFactory factory)
    {
        ruleBuilder.NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
        ruleBuilder.MustAsync((command, name, cancellationToken) =>
                IsEmailUnique(getId(command), name, factory, cancellationToken))
            .WithMessage("EmailAddress is already taken");
    }

    private static async Task<bool> IsEmailUnique(UserId? id, string email, IServiceScopeFactory factory,
        CancellationToken cancellationToken)
    {
        using var scope = factory.CreateScope();
        var repository = scope.Resolve<IRepository<User>>();
        return !await repository.QueryAll()
            .QueryExceptWithIdOptional(id)
            .QueryByEmail(email)
            .AnyAsync(cancellationToken);
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