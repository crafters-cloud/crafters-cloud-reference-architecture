using Ardalis.SmartEnum;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

public class UserStatusId(string name, byte value) : SmartEnum<UserStatusId>(name, value)
{
    public static readonly UserStatusId Active = new(nameof(Active), 1);
    public static readonly UserStatusId Inactive = new(nameof(Inactive), 2);
}