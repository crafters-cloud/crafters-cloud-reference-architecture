using CraftersCloud.Core.TestUtilities.Database;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Respawn;
using Respawn.Graph;
using Testcontainers.MsSql;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Database;

internal class TestDatabase
{
    public string ConnectionString { get; private set; } = null!;
    private static MsSqlContainer? _container;

    private static readonly IEnumerable<string> TablesToIgnore =
        ["__EFMigrationsHistory", nameof(Role), nameof(RolePermission), nameof(Permission), nameof(UserStatus)];

    public async Task CreateAsync()
    {
        // To use a local sqlServer instance, Create an Environment variable using R# Test Runner, with name "IntegrationTestsConnectionString"
        // and value: "Server=.;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
        var connectionString = Environment.GetEnvironmentVariable("IntegrationTestsConnectionString");
        connectionString = string.Empty;

        if (!string.IsNullOrEmpty(connectionString))
        {
            // do not write ConnectionString to the console since it might contain username/password 
            ConnectionString = connectionString;
        }
        else
        {
            try
            {
                // These cannot be changed (it is hardcoded in MsSqlBuilder and changing any of them breaks starting of the container
                // default database: master
                // default username: sa
                // default password: yourStrong(!)Password

                _container ??= new MsSqlBuilder()
                    .WithAutoRemove(true)
                    .WithCleanUp(true)
                    .Build();

                await _container!.StartAsync();
                ConnectionString = _container.GetConnectionString();
                WriteLine($"Docker SQL connection string: {ConnectionString}");
            }
            catch (Exception e)
            {
                WriteLine($"Failed to start docker container: {e.Message}");
                throw;
            }
        }
    }

    public static Task ResetAsync(DbContext dbContext) => DatabaseInitializer2.RecreateDatabaseAsync(dbContext,
        new DatabaseInitializerOptions
        {
            ResetDataOptions = new ResetDataOptions
            {
                TablesToIgnore = TablesToIgnore
            }
        });

    private static void WriteLine(string value) => TestContext.WriteLine(value);
}


[PublicAPI]
public static class DatabaseInitializer2
{
    public static async Task RecreateDatabaseAsync(DbContext dbContext, DatabaseInitializerOptions options)
    {
        if (HasSchemaChanges(dbContext))
        {
            RecreateDatabase(dbContext);
        }
        else
        {
            await ResetDataAsync(dbContext, options.ResetDataOptions);
        }
    }

    private static bool HasSchemaChanges(DbContext dbContext)
    {
        try
        {
            var dbDoesNotExist =
                !dbContext.Database
                    .CanConnect(); // this will throw SqlException if connection to server can not be made, and true / false depending if db exists
            return dbDoesNotExist || dbContext.Database.GetPendingMigrations().Any();
        }
        catch (SqlException ex)
        {
            WriteLine("Error connecting to SqlServer:");
            WriteLine(ex.ToString());
            throw;
        }
    }

    private static void RecreateDatabase(DbContext dbContext)
    {
        DropAllDbObjects(dbContext.Database);
        dbContext.Database.Migrate();
    }

    private static async Task ResetDataAsync(DbContext dbContext, ResetDataOptions options)
    {
        await DeleteDataAsync(dbContext, options);
        RunCustomQuery(dbContext, options.CustomSqlQuery);
    }

    private static async Task DeleteDataAsync(DbContext dbContext, ResetDataOptions options)
    {
        var connectionString = dbContext.Database.GetConnectionString() ?? string.Empty;

        var respawner = await Respawner.CreateAsync(connectionString,
            new RespawnerOptions
            {
                TablesToIgnore = options.TablesToIgnore.Select(name => new Table(name)).ToArray(),
                WithReseed = options.ReseedIdentityColumns
            });

        await respawner.ResetAsync(connectionString);
    }

    private static void RunCustomQuery(DbContext dbContext, string customSqlQuery)
    {
        if (!string.IsNullOrEmpty(customSqlQuery))
        {
            dbContext.Database.ExecuteSqlRaw(customSqlQuery);
        }
    }

    private static void DropAllDbObjects(DatabaseFacade database)
    {
        try
        {
            var dropAllSql = DatabaseHelpers.DropAllSql;
            foreach (var statement in dropAllSql.SplitStatements())
            {
                database.ExecuteSqlRaw(statement);
            }
        }
        catch (SqlException ex)
        {
            const int cannotOpenDatabaseErrorNumber = 4060;
            if (ex.Number == cannotOpenDatabaseErrorNumber)
            {
                WriteLine("Error while trying to drop all objects from database. Maybe database does not exist.");
                WriteLine("Continuing...");
                WriteLine(ex.ToString());
            }
            else
            {
                throw;
            }
        }
    }

    private static void WriteLine(string message) => TestContext.WriteLine(message);
}