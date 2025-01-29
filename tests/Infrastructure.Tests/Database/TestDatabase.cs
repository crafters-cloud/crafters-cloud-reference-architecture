using CraftersCloud.Core.Tests.Shared.Database;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Products;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Database;

public class TestDatabase
{
    public string ConnectionString { get; private set; } = null!;
    private static MsSqlContainer? _container;

    private static readonly IEnumerable<string> TablesToIgnore =
    [
        "__EFMigrationsHistory", nameof(Role), nameof(RolePermission), nameof(Permission), nameof(UserStatus),
        nameof(ProductStatus)
    ];

    public async Task CreateAsync()
    {
        try
        {
            _container ??= new MsSqlBuilder()
                // Resource reuse for better development experience https://dotnet.testcontainers.org/api/resource_reuse/
                .WithReuse(true)
                .WithName("reference-architecture-sql-integration-tests")
                .WithLabel("reuse-id", "reference-architecture-sql-integration-tests")
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

    public static Task ResetAsync(DbContext dbContext) => DatabaseInitializer.RecreateDatabaseAsync(dbContext,
        new DatabaseInitializerOptions
        {
            ResetDataOptions = new ResetDataOptions
            {
                TablesToIgnore = TablesToIgnore
            }
        });

    private static void WriteLine(string value) => TestContext.Out.Write(value);
}