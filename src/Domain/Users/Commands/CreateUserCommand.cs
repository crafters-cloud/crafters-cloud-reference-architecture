using CraftersCloud.Core.Data;
using CraftersCloud.Core.Messaging;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

[PublicAPI]
public class CreateUserCommand : ICommand<CreateCommandResult<User>>
{
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateUserCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
            RuleFor(x => x.EmailAddress).MustAsync(UniqueEmailAddress).WithMessage("EmailAddress is already taken");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(User.FirstNameMaxLength);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(User.LastNameMaxLength);
            RuleFor(x => x.RoleId).NotEmpty();
        }

        private async Task<bool> UniqueEmailAddress(CreateUserCommand command, string name, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
            return !await userRepository.QueryAll()
                .QueryByEmail(name)
                .AnyAsync(ct);
        }
    }
}