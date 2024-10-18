using CraftersCloud.ReferenceArchitecture.Domain.Auditing;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

public record UserCreatedDomainEvent(string EmailAddress) : AuditableDomainEvent("UserCreated")
{
    public override object AuditPayload => new { EmailAddress };
}