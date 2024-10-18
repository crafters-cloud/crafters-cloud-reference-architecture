using CraftersCloud.Core.TestUtilities.Database;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Database;

internal class TestDatabase
{
    public string ConnectionString { get; }
    private static MsSqlContainer? _container;

    private static readonly IEnumerable<string> TablesToIgnore =
        ["__EFMigrationsHistory", nameof(Role), nameof(RolePermission), nameof(Permission), nameof(UserStatus)];

    public TestDatabase()
    {
        // To use a local sqlServer instance, Create an Environment variable using R# Test Runner, with name "IntegrationTestsConnectionString"
        // and value: "Server=.;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
        var connectionString = Environment.GetEnvironmentVariable("IntegrationTestsConnectionString");

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

                _container!.StartAsync().Wait();
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

    public static Task ResetAsync(DbContext dbContext) => DatabaseInitializer.RecreateDatabaseAsync(dbContext,
        new DatabaseInitializerOptions
        {
            ResetDataOptions = new ResetDataOptions
            {
                TablesToIgnore = TablesToIgnore
            }
        });

    private static void WriteLine(string value) => TestContext.WriteLine(value);
}