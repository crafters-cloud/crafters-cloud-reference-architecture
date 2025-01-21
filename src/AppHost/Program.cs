using CraftersCloud.ReferenceArchitecture.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var dataDirectory = Path.Combine(AppDomain.CurrentDomain.GetDirectoryPath(5), "data");

var cache = builder.AddRedis("redis")
    .WithDataBindMount(Path.Combine(dataDirectory, "redis"))
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisCommander();

var sqlServer = builder.AddSqlServer("sql-server", port: 1533)
    .WithDataBindMount(Path.Combine(dataDirectory, "sql-server"))
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("AppDbContext");

var migrations = builder.AddProject<MigrationService>("migrations")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Api>("api")
    .WithReference(cache)
    .WithReference(database)
    .WaitFor(migrations);

await builder.Build().RunAsync();