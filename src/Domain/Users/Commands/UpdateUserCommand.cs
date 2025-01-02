using CraftersCloud.Core;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Messaging;
using CraftersCloud.ReferenceArchitecture.Core.CommandResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

[PublicAPI]
public class UpdateUserCommand : ICommand<UpdateCommandResult<User>>
{
    public Guid Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
            RuleFor(x => x.EmailAddress).MustAsync(UniqueEmailAddress).WithMessage("EmailAddress is already taken");
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(User.NameMaxLength);
            RuleFor(x => x.RoleId).NotEmpty();
        }

        private async Task<bool> UniqueEmailAddress(UpdateUserCommand command, string name, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.Resolve<IRepository<User>>();
            return !await userRepository.QueryAll()
                .QueryByEmailAddress(name)
                .AnyAsync(ct);
        }
    }
}