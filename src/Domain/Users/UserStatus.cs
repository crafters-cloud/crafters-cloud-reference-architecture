using CraftersCloud.Core.SmartEnums.Entities;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UserStatus : EntityWithEnumId<UserStatusId>
{
    public const int NameMaxLength = 50;

    public string Description { get; private set; } = string.Empty;
}