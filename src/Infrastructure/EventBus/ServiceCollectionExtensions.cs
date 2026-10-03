using System.Data.Common;
using System.Text.Json;
using CraftersCloud.Core.IntegrationEvents;
using CraftersCloud.Core.IntegrationEvents.IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.EventBus;

public static class ServiceCollectionExtensions
{
    public static void AppAddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIntegrationEventService, IntegrationEventService>();
        services.AddScoped<IIntegrationEventLogService, IntegrationEventLogService>();
    }
}

public class IntegrationEventLogService(IntegrationEventLogDbContext dbContext) : IIntegrationEventLogService
{
    public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
    {
        var eventLogEntry = new IntegrationEventLogEntry(@event, o => JsonSerializer.Serialize(o));

        if (dbContext.Database.CurrentTransaction == null)
        {
            dbContext.Database.UseTransaction(transaction);
        }

        dbContext.IntegrationEventLogs.Add(eventLogEntry);
        return dbContext.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(IntegrationEvent @event)
    {
        var eventLogEntry =
            dbContext.IntegrationEventLogs.Single(ie => ie.EventId == @event.Id);
        eventLogEntry.TimesSent++;
        eventLogEntry.State = EventState.Published;

        dbContext.IntegrationEventLogs.Update(eventLogEntry);
        return dbContext.SaveChangesAsync();
    }
}

public class IntegrationEventService : IIntegrationEventService
{
    public Task PublishThroughEventBusAsync<T>(T evt) where T : IntegrationEvent =>
        // do nothing, yet
        Task.CompletedTask;
}