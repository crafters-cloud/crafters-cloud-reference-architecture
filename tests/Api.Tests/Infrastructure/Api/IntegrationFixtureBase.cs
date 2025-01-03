using System.Text.Json;
using CraftersCloud.Core;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.TestUtilities;
using CraftersCloud.ReferenceArchitecture.Infrastructure;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Configuration;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Database;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;

public class IntegrationFixtureBase
{
    private IConfiguration _configuration = null!;
    private TestDatabase _testDatabase = null!;
    private IServiceScope _testScope = null!;
    private static ApiWebApplicationFactory _factory = null!;
    private bool _isUserAuthenticated = true;
    protected FlurlClient Client { get; private set; } = null!;

    [SetUp]
    protected async Task Setup()
    {
        _testDatabase = new TestDatabase();
        await _testDatabase.CreateAsync();

        _configuration = new TestConfigurationBuilder()
            .WithDbContextName(nameof(AppDbContext))
            .WithConnectionString(_testDatabase.ConnectionString)
            .Build();

        _factory = new ApiWebApplicationFactory(_configuration, _isUserAuthenticated);

        var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _testScope = scopeFactory.CreateScope();
        var httpClient = _factory.CreateClient();
        Client = CreateFlurlClient(httpClient);

        var dbContext = Resolve<DbContext>();
        await TestDatabase.ResetAsync(dbContext);

        SeedTestUsers();
    }

    protected void DisableUserAuthentication() => _isUserAuthenticated = false;

    private void SeedTestUsers()
    {
        var dbContext = Resolve<DbContext>();
        new TestUserDataSeeding(dbContext).Seed();
    }

    [TearDown]
    public void Teardown()
    {
        _factory.Dispose();
        _testScope.Dispose();
        Client.Dispose();
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

    protected T QueryByIdSkipCache<T>(Guid id) where T : EntityWithTypedId<Guid> =>
        QueryDbSkipCache<T>().QueryById(id).Single();

    protected T QueryByIdSkipCache<T>(int id) where T : EntityWithTypedId<int> =>
        QueryDbSkipCache<T>().QueryById(id).Single();

    protected IQueryable<T> QueryDbSkipCache<T>() where T : class => Resolve<DbContext>().QueryDbSkipCache<T>();

    private Task DeleteByIdAsync<T, TId>(TId id) where T : class => Resolve<DbContext>().DeleteByIdAsync<T, TId>(id);

    protected T Resolve<T>() where T : notnull => _testScope.Resolve<T>();

    protected void SetNow(DateTimeOffset value)
    {
        var settableTimeProvider = Resolve<TestTimeProvider>();
        settableTimeProvider.SetNow(value);
    }
    
    private static FlurlClient CreateFlurlClient(HttpClient httpClient) =>
        new FlurlClient(httpClient).WithSettings(settings =>
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.Converters.AppRegisterJsonConverters([AssemblyFinder.ApiAssembly]);
            settings.JsonSerializer = new DefaultJsonSerializer(options);
        });
}