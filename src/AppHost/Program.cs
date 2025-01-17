var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

const string databaseName = "app-db";

var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("SQL_SERVER", databaseName);

var database = sql.AddDatabase(databaseName);

builder.AddProject<Projects.MigrationService>("migrations")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.Api>("api")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(database)
    .WaitFor(database);

await builder.Build().RunAsync();