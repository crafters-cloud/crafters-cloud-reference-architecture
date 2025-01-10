using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;

[UsedImplicitly]
public class CurrentUserProvider(
    IClaimsProvider claimsProvider,
    IRepository<User, UserId> userRepository,
    ILogger<CurrentUserProvider> logger)
    : ICurrentUserProvider
{
    private UserContext? _user;
    private bool IsAuthenticated => claimsProvider.IsAuthenticated;
    public UserId? UserId => User?.UserId;

    public UserContext? User
    {
        get
        {
            if (_user != null)
            {
                return _user;
            }

            if (!IsAuthenticated)
            {
                logger.LogWarning("User is not authenticated");
                return null;
            }

            if (string.IsNullOrEmpty(claimsProvider.Email))
            {
                logger.LogWarning("User's email was not found in the claims");
                return null;
            }

            var user = userRepository
                .QueryAll()
                .QueryByEmail(claimsProvider.Email)
                .IncludeAggregate()
                .AsNoTracking()
                .AsSplitQuery()
                .SingleOrDefault();

            _user = user != null
                ? new UserContext(user.Id,
                    new PermissionsContext(user.Role.Permissions.Select(x => x.Id).Distinct().ToArray()))
                : null;

            return _user;
        }
    }
}