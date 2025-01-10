using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;

[UsedImplicitly]
public class TestUserProvider(IRepository<User, UserId> userRepository, ILogger<TestUserProvider> logger)
    : SystemUserProvider(userRepository, logger)
{
    public override UserId? UserId => TestUserData.TestUserId;
}