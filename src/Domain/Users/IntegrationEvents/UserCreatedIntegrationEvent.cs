using CraftersCloud.Core.IntegrationEvents;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.IntegrationEvents;

public class UserCreatedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
}