using Azure.Provisioning.ServiceBus;
using CraftersCloud.ReferenceArchitecture.AppHost;
using Projects;

const string projectName = "reference-architecture";
var dataDirectory = Path.Combine(AppDomain.CurrentDomain.GetDirectoryPath(5), "data");

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis")
    .WithContainerName($"{projectName}-redis")
    .WithDataBindMount(Path.Combine(dataDirectory, "redis"))
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisCommander(
        c => c.WithContainerName($"{projectName}-redis-commander"));

var sqlServer = builder.AddSqlServer("sql-server", port: 1533)
    .WithContainerName($"{projectName}-sql-server")
    .WithDataBindMount(Path.Combine(dataDirectory, "sql-server"))
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("app-db");

var migrations = builder.AddProject<MigrationService>("migrations")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Api>("api")
    .WithReference(cache)
    .WithReference(database)
    .WaitFor(migrations);

var serviceBus = builder.AddAzureServiceBus("sbemulator");

serviceBus.AddQueue("queue1")
    .WithProperties(queue => queue.DeadLetteringOnMessageExpiration = false);

serviceBus
    .WithQueue("queue1", queue =>
    {
        queue.DeadLetteringOnMessageExpiration = false;
    })
    .WithTopic("topic1", topic =>
    {
        var subscription = new ServiceBusSubscription("sub1")
        {
            MaxDeliveryCount = 10,
        };
        topic.Subscriptions.Add(subscription);

        var rule = new ServiceBusRule("app-prop-filter-1")
        {
            CorrelationFilter = new()
            {
                ContentType = "application/text",
                CorrelationId = "id1",
                Subject = "subject1",
                MessageId = "msgid1",
                ReplyTo = "someQueue",
                ReplyToSessionId = "sessionId",
                SessionId = "session1",
                SendTo = "xyz"
            }
        };
        subscription.Rules.Add(rule);
    })
    ;

await builder.Build().RunAsync();