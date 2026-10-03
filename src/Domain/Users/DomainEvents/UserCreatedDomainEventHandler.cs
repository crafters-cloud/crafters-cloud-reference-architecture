using CraftersCloud.Core.IntegrationEvents;
using CraftersCloud.ReferenceArchitecture.Domain.Users.IntegrationEvents;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;

public class UserCreatedDomainEventHandler(IIntegrationEventService eventService)
    : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new UserCreatedIntegrationEvent
            { Id = notification.Id, EmailAddress = notification.EmailAddress };
        await eventService.PublishThroughEventBusAsync(integrationEvent);
    }
}