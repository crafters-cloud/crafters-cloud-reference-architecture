using CraftersCloud.ReferenceArchitecture.Domain.Auditing;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

public record UserUpdatedDomainEvent(Guid Id, string EmailAddress) : AuditableDomainEvent("UserUpdated")
{
    public override object AuditPayload => new { Id, EmailAddress };
}