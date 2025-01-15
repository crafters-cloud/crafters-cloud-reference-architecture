using CraftersCloud.Core;
using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

[PublicAPI]
public class UpdateUserCommand : ICommand<UpdateCommandResult<User>>
{
    public UserId Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public RoleId RoleId { get; set; }
    public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.EmailAddress).ValidateUserEmail(UniqueEmailAddress);
            RuleFor(x => x.FirstName).ValidateUserFirstName();
            RuleFor(x => x.LastName).ValidateUserLastName();
            RuleFor(x => x.RoleId).ValidateRoleId();
        }

        private async Task<bool> UniqueEmailAddress(UpdateUserCommand command, string name,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<User>>();
            return !await repository.QueryAll()
                .QueryByEmail(name)
                .AnyAsync(cancellationToken);
        }
    }
}