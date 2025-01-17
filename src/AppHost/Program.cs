var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

const string databaseName = "app-db";

var sqlServer = builder.AddSqlServer("sql-server")
    .WithEnvironment("SQL_SERVER", databaseName);

var database = sqlServer.AddDatabase(databaseName);

builder.AddProject<Projects.Api>("api")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(database)
    .WaitFor(database);

await builder.Build().RunAsync();