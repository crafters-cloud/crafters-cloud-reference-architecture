using CraftersCloud.ReferenceArchitecture.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;
using CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

public class User : EntityWithCreatedUpdated
{
    public const int NameMaxLength = 200;
    public const int EmailAddressMaxLength = 200;
    public static readonly Guid SystemUserId = new("DFB44AA8-BFC9-4D95-8F45-ED6DA241DCFC");
    public string EmailAddress { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public UserStatusId UserStatusId { get; private set; } = null!;
    public UserStatus UserStatus { get; private set; } = null!;

    public static User Create(CreateUserCommand command)
    {
        var result = new User
        {
            EmailAddress = command.EmailAddress, FullName = command.FullName, RoleId = command.RoleId,
            UserStatusId = command.UserStatusId
        };

        result.AddDomainEvent(new UserCreatedDomainEvent(result.EmailAddress));
        return result;
    }

    public void Update(UpdateUserCommand command)
    {
        FullName = command.FullName;
        RoleId = command.RoleId;
        UserStatusId = command.UserStatusId;
        AddDomainEvent(new UserUpdatedDomainEvent(EmailAddress));
    }

    public void UpdateRole(Role role)
    {
        RoleId = role.Id;
        Role = role;
        AddDomainEvent(new UserUpdatedDomainEvent(EmailAddress));
    }

    public IReadOnlyCollection<PermissionId> GetPermissionIds() => Role.Permissions.Select(p => p.Id).ToArray();
}