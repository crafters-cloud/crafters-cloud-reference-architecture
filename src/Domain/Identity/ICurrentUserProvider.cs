using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Domain.Identity;

public interface ICurrentUserProvider
{
    UserContext? User { get; }
    UserId? UserId { get; }
}