using CraftersCloud.Core.Cqrs;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

[PublicAPI]
public class CreateUserCommand : ICommand<CreateCommandResult<User>>
{
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public RoleId RoleId { get; set; }
    public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateUserCommand>
    {
        public Validator(IServiceScopeFactory scopeFactory)
        {
            RuleFor(x => x.EmailAddress).ValidateUserEmail(x => null, scopeFactory);
            RuleFor(x => x.FirstName).ValidateUserFirstName();
            RuleFor(x => x.LastName).ValidateUserLastName();
            RuleFor(x => x.RoleId).ValidateRoleId();
        }
    }
}