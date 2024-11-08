using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using FluentValidation;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public static class CreateOrUpdateUser
{
    [PublicAPI]
    public class Command : ICommand<User>
    {
        public required Guid? Id { get; set; }
        public required string EmailAddress { get; set; } = string.Empty;
        public required string FullName { get; set; } = string.Empty;
        public required Guid RoleId { get; set; }
        public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;
    }

    [UsedImplicitly]
    public class Validator : AbstractValidator<Command>
    {
        private readonly IRepository<User> _userRepository;

        public Validator(IRepository<User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(User.EmailAddressMaxLength).EmailAddress();
            RuleFor(x => x.EmailAddress).Must(UniqueEmailAddress).WithMessage("EmailAddress is already taken");
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(User.NameMaxLength);
            RuleFor(x => x.RoleId).NotEmpty();
        }

        private bool UniqueEmailAddress(Command command, string name) =>
            !_userRepository.QueryAll()
                .QueryExceptWithId(command.Id)
                .QueryByEmailAddress(name)
                .Any();
    }
}