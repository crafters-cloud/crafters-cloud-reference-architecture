using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Domain.Identity;

public class UserContext(UserId userId, PermissionsContext permissions)
{
    public UserId UserId => userId;
    public PermissionsContext Permissions => permissions;
}