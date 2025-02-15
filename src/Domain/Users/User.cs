﻿using CraftersCloud.Core.StronglyTypedIds;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;
using CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

[StronglyTypedId(ValueKind.Guid)]
public readonly partial record struct UserId;

public class User : EntityWithCreatedUpdated<UserId>
{
    public const int FirstNameMaxLength = 200;
    public const int LastNameMaxLength = 200;
    public const int EmailAddressMaxLength = 200;

    public static readonly UserId SystemUserId = UserId.Create(new Guid("DFB44AA8-BFC9-4D95-8F45-ED6DA241DCFC"));
    public string EmailAddress { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public RoleId RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
    public UserStatusId UserStatusId { get; private set; } = null!;
    public UserStatus UserStatus { get; private set; } = null!;

    private User()
    {
    }

    public static User Create(CreateUserCommand command)
    {
        var result = new User
        {
            Id = UserId.CreateNew(),
            EmailAddress = command.EmailAddress, FirstName = command.FirstName, LastName = command.LastName,
            RoleId = command.RoleId,
            UserStatusId = command.UserStatusId
        };

        result.AddDomainEvent(new UserCreatedDomainEvent(result.Id, result.EmailAddress));
        return result;
    }

    public void Update(UpdateUserCommand command)
    {
        FirstName = command.FirstName;
        LastName = command.LastName;
        RoleId = command.RoleId;
        UserStatusId = command.UserStatusId;
        AddDomainEvent(new UserUpdatedDomainEvent(Id, EmailAddress));
    }

    public void UpdateRole(Role role)
    {
        RoleId = role.Id;
        Role = role;
        AddDomainEvent(new UserUpdatedDomainEvent(Id, EmailAddress));
    }

    public IReadOnlyCollection<PermissionId> GetPermissionIds() => Role.Permissions.Select(p => p.Id).ToArray();
}