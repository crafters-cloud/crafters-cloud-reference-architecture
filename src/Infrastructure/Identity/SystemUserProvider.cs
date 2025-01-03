using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DomainUser = CraftersCloud.ReferenceArchitecture.Domain.Users.User;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;

[UsedImplicitly]
public class SystemUserProvider(IRepository<DomainUser> userRepository, ILogger<SystemUserProvider> logger)
    : ICurrentUserProvider
{
    public virtual Guid? UserId => DomainUser.SystemUserId;
    private UserContext? _user;

    public UserContext? User
    {
        get
        {
            if (_user != null)
            {
                return _user;
            }

            var user = userRepository
                .QueryAll()
                .QueryById(UserId!.Value)
                .IncludeAggregate()
                .AsNoTracking()
                .SingleOrDefault();

            if (user == null)
            {
                logger.LogWarning(
                    "User with Id: {SystemUserId} not found in the database. This user might not have been seeded",
                    UserId);
                return null;
            }

            _user = new UserContext(user.Id, new PermissionsContext(user.GetPermissionIds()));

            return _user;
        }
    }
}