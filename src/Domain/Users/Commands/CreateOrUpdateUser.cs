using Ardalis.Result;
using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public static partial class CreateOrUpdateUser
{
    [PublicAPI]
    public class Command : ICommand<Result<User>>
    {
        public Guid? Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;
    }

    [UsedImplicitly]
    public class Validator : AbstractValidator<Command>
    {
        private readonly IServiceScopeFactory  _scopeFactory;

        public Validator(IServiceScopeFactory  scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
            RuleFor(x => x.EmailAddress).MustAsync(UniqueEmailAddress).WithMessage("EmailAddress is already taken");
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(User.NameMaxLength);
            RuleFor(x => x.RoleId).NotEmpty();
        }

        private async Task<bool> UniqueEmailAddress(Command command, string name, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
            return !await userRepository.QueryAll()
                .QueryExceptWithId(command.Id)
                .QueryByEmailAddress(name)
                .AnyAsync(cancellationToken: ct);
        }
    }
}