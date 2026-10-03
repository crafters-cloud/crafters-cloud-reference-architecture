using CraftersCloud.Core.IntegrationEvents;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.IntegrationEvents;

public class UserUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public int UserStatusId { get; set; }
    public Guid RoleId { get; set; }
}