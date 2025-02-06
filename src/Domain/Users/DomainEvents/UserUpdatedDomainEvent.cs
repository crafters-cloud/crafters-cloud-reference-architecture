using CraftersCloud.ReferenceArchitecture.Core.Caching;
using CraftersCloud.ReferenceArchitecture.Domain.Auditing;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

public record UserUpdatedDomainEvent(UserId Id, string EmailAddress)
    : AuditableDomainEvent("UserUpdated"), ICacheTagEvictor
{
    public override object AuditPayload => new { Id, EmailAddress };
    public string[] Tags => [UserCacheTags.Users, UserCacheTags.User(Id)];
}