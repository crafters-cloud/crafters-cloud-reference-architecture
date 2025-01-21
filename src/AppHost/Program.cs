using CraftersCloud.ReferenceArchitecture.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var dataDirectory = Path.Combine(AppDomain.CurrentDomain.GetDirectoryPath(5), "data");

var cache = builder.AddRedis("redis")
    .WithDataBindMount(Path.Combine(dataDirectory, "redis"))
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisCommander();

var sqlServer = builder.AddSqlServer("sql-server")
    .WithDataBindMount(Path.Combine(dataDirectory, "sql-server"))
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("app-db");

builder.AddProject<MigrationService>("migrations")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Api>("api")
    .WithReference(cache)
    .WithReference(database);

await builder.Build().RunAsync();