using CraftersCloud.Core.IntegrationEvents;
using CraftersCloud.ReferenceArchitecture.Domain.Users.IntegrationEvents;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

public class UserUpdatedDomainEventHandler(IIntegrationEventService eventService)
    : INotificationHandler<UserUpdatedDomainEvent>
{
    public async Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new UserUpdatedIntegrationEvent
            { Id = notification.Id, RoleId = notification.RoleId, UserStatusId = notification.StatusId };
        await eventService.PublishThroughEventBusAsync(integrationEvent);
    }
}