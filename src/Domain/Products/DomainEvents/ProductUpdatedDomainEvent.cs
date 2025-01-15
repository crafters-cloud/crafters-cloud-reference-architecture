using CraftersCloud.ReferenceArchitecture.Domain.Auditing;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.DomainEvents;

public record ProductUpdatedDomainEvent(ProductId Id, string EmailAddress) : AuditableDomainEvent("ProductUpdated")
{
    public override object AuditPayload => new { Id, EmailAddress };
}