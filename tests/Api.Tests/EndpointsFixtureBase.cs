using CraftersCloud.Core;
using CraftersCloud.Core.Tests.Shared;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Cache;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Configuration;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Database;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests;

public abstract class EndpointsFixtureBase
{
    private IConfiguration _configuration = null!;
    private TestDatabase _testDatabase = null!;
    private IServiceScope _testScope = null!;
    private static ApiWebApplicationFactory<Program> _factory = null!;
    private bool _isUserAuthenticated = true;
    private TestCache _testCache;
    protected FlurlClient Client { get; private set; } = null!;

    [SetUp]
    protected async Task Setup()
    {
        _testDatabase = new TestDatabase();
        await _testDatabase.CreateAsync();

        _testCache = new TestCache();
        await _testCache.CreateAsync();

        _configuration = new TestConfigurationBuilder()
            .WithDbName("app-db")
            .WithDbConnectionString(_testDatabase.ConnectionString)
            .WithCacheConnectionString(_testCache.ConnectionString)
            .Build();

        _factory = new ApiWebApplicationFactory<Program>(_configuration, _isUserAuthenticated);

        var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _testScope = scopeFactory.CreateScope();
        Client = _factory.CreateClient().ToFlurlClient();

        var dbContext = Resolve<DbContext>();
        await TestDatabase.ResetAsync(dbContext);

        SeedTestUsers(dbContext);
    }

    protected void DisableUserAuthentication() => _isUserAuthenticated = false;

    private static void SeedTestUsers(DbContext dbContext) => new TestUserDataSeeding(dbContext).Seed();

    [TearDown]
    public void Teardown()
    {
        Client.Dispose();
        _testScope.Dispose();
        _factory.Dispose();
    }

    protected void AddAndSaveChanges(params object[] entities)
    {
        var dbContext = Resolve<DbContext>();
        dbContext.AddRange(entities);
        dbContext.SaveChanges();
    }

    protected void AddToDbContext(params object[] entities)
    {
        var dbContext = Resolve<DbContext>();
        dbContext.AddRange(entities);
    }

    protected Task SaveChangesAsync() => Resolve<DbContext>().SaveChangesAsync();

    protected IQueryable<T> QueryDb<T>() where T : class => Resolve<DbContext>().Set<T>();

    protected IQueryable<T> QueryDbSkipCache<T>() where T : class => Resolve<DbContext>().QueryDbSkipCache<T>();

    private Task DeleteByIdAsync<T, TId>(TId id) where T : class => Resolve<DbContext>().DeleteByIdAsync<T, TId>(id);

    protected T Resolve<T>() where T : notnull => _testScope.Resolve<T>();

    protected void SetNow(DateTimeOffset value)
    {
        var settableTimeProvider = Resolve<TestTimeProvider>();
        settableTimeProvider.SetNow(value);
    }
}