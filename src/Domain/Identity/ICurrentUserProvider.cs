namespace CraftersCloud.ReferenceArchitecture.Domain.Identity;

public interface ICurrentUserProvider
{
    UserContext? User { get; }
    Guid? UserId { get; }
}