using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;

public class UserBuilder
{
    private string _fullName = string.Empty;
    private Guid _roleId = Guid.Empty;
    private string _emailAddress = string.Empty;
    private Guid _id = SequentialGuidGenerator.Generate();
    private UserStatusId _statusId = UserStatusId.Active;

    public UserBuilder WithEmailAddress(string value)
    {
        _emailAddress = value;
        return this;
    }

    public UserBuilder WithFullName(string value)
    {
        _fullName = value;
        return this;
    }

    public UserBuilder WithRoleId(Guid value)
    {
        _roleId = value;
        return this;
    }

    public UserBuilder WithId(Guid value)
    {
        _id = value;
        return this;
    }

    public UserBuilder WithStatusId(UserStatusId value)
    {
        _statusId = value;
        return this;
    }

    public static implicit operator User(UserBuilder builder) => ToUser(builder);

    public static User ToUser(UserBuilder builder) => builder.Build();

    public User Build()
    {
        var result = User.Create(new CreateOrUpdateUser.Command
        {
            Id = _id,
            FullName = _fullName,
            EmailAddress = _emailAddress,
            RoleId = _roleId,
            UserStatusId = _statusId
        });

        return result;
    }
}