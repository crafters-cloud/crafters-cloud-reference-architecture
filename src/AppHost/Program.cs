using System.Text.Json.Nodes;
using Aspire.Hosting.Azure;
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

var serviceBus = builder.AddAzureServiceBus("sbemulator");

serviceBus.AddServiceBusQueue("queue1")
    .WithProperties(queue => queue.DeadLetteringOnMessageExpiration = false);

serviceBus.AddServiceBusTopic("topic1")
    .AddServiceBusSubscription("sub1")
    .WithProperties(subscription =>
    {
        subscription.MaxDeliveryCount = 10;

        var rule = new AzureServiceBusRule("app-prop-filter-1")
        {
            CorrelationFilter = new AzureServiceBusCorrelationFilter
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
    });

serviceBus.RunAsEmulator(configure => configure.WithContainerName($"{projectName}-service-bus")
    .WithConfiguration(document =>
    {
        document["UserConfig"]!["Logging"] = new JsonObject { ["Type"] = "Console" };
    }).WithLifetime(ContainerLifetime.Persistent));

builder.AddProject<Api>("api")
    .WithReference(cache)
    .WithReference(database)
    .WaitFor(migrations);

builder.AddProject<Projects.ServiceBusWorker>("worker")
    .WithReference(serviceBus).WaitFor(serviceBus);

await builder.Build().RunAsync();