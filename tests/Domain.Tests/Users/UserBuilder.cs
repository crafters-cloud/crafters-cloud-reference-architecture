using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;

public class UserBuilder
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private RoleId _roleId = new(Guid.Empty);
    private string _emailAddress = string.Empty;
    private UserStatusId _statusId = UserStatusId.Active;

    public UserBuilder WithEmailAddress(string value)
    {
        _emailAddress = value;
        return this;
    }

    public UserBuilder WithFirstName(string value)
    {
        _firstName = value;
        return this;
    }

    public UserBuilder WithLastName(string value)
    {
        _lastName = value;
        return this;
    }

    public UserBuilder WithRoleId(RoleId value)
    {
        _roleId = value;
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
        var result = User.Create(new CreateUserCommand
        {
            FirstName = _firstName,
            LastName = _lastName,
            EmailAddress = _emailAddress,
            RoleId = _roleId,
            UserStatusId = _statusId
        });

        return result;
    }
}