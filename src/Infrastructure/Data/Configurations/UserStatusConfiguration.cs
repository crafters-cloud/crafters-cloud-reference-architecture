using CraftersCloud.ReferenceArchitecture.Domain.Users;
using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UserStatusConfiguration : EntityWithEnumIdConfiguration<UserStatus, UserStatusId>;