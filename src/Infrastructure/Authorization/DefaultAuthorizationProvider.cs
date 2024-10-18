using CraftersCloud.Core.AspNetCore.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;

public class DefaultAuthorizationProvider(ICurrentUserProvider currentUserProvider)
    : IAuthorizationProvider<PermissionId>
{
    public bool AuthorizePermissions(IEnumerable<PermissionId> permissions) =>
        currentUserProvider.User?.Permissions.ContainsAny(permissions) ?? false;
}