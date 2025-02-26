using Azure.Messaging.ServiceBus;

namespace CraftersCloud.ReferenceArchitecture.ServiceBusWorker;

internal sealed class Producer(ServiceBusClient client, ILogger<Producer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting producer...");

        await using var sender = client.CreateSender("queue1");

        var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (!stoppingToken.IsCancellationRequested)
        {
            await periodicTimer.WaitForNextTickAsync(stoppingToken);

            await sender.SendMessageAsync(new ServiceBusMessage($"Hello, World! It's {DateTime.Now} here."), stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping producer...");
        return Task.CompletedTask;
    }
}
