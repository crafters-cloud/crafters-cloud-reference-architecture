using CraftersCloud.Core.SmartEnums.Entities;
using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UserStatus : EntityWithEnumId<UserStatusId>
{
    public string Description { get; private set; } = string.Empty;
};