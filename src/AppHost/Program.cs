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

await builder.Build().RunAsync();