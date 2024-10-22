using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;

[UsedImplicitly]
public class TestUserProvider(IRepository<User> userRepository, ILogger<TestUserProvider> logger)
    : SystemUserProvider(userRepository, logger)
{
    public override Guid? UserId => TestUserData.TestUserId;
}