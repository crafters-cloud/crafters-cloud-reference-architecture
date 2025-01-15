using CraftersCloud.ReferenceArchitecture.Domain.Auditing;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products.DomainEvents;

public record ProductCreatedDomainEvent(ProductId Id, string Name) : AuditableDomainEvent("ProductCreated")
{
    public override object AuditPayload => new { Id, Name };
}