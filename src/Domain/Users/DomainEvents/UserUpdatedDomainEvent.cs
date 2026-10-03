using CraftersCloud.Core.Caching.Abstractions;
using CraftersCloud.ReferenceArchitecture.Domain.Auditing;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

public record UserUpdatedDomainEvent(UserId Id, RoleId RoleId, UserStatusId StatusId)
    : AuditableDomainEvent("UserUpdated"), ICacheEvictor
{
    public override object AuditPayload => new { Id, RoleId, UserStatusId = StatusId };
    public string[] Tags => [UserCacheTags.Users, UserCacheTags.User(Id)];
}