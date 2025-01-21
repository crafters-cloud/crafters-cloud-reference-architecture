using System.Net;
using Aspire.Hosting;
using CraftersCloud.Core.AspireTests.Shared;

namespace CraftersCloud.ReferenceArchitecture.AppHost.Tests;

/// <summary>
/// Verifies that the application host starts and runs correctly.
/// </summary>
[Category("aspire-integration")]
public class AppHostSetupFixture
{
    private DistributedApplication _app;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var appHost = await DistributedApplicationTestFactory.CreateAsync<Projects.AppHost>();
        
        _app = await appHost.BuildAsync().WaitAsync(TimeSpan.FromSeconds(15));
        await _app.StartAsync().WaitAsync(TimeSpan.FromSeconds(120));
        await _app.WaitForResourcesAsync().WaitAsync(TimeSpan.FromSeconds(120));
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _app.StopAsync().WaitAsync(TimeSpan.FromSeconds(15));
        await _app.DisposeAsync();
    }

    [Test]
    public void AppHostRunsCleanly() => _app.EnsureNoErrorsLogged();

    [Test]
    public async Task ApiIsOn()
    {
        var httpClient = _app.CreateHttpClient("api");
        var response = await httpClient.GetAsync("/profile/hello-world");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [TestCase("AppDbContext", TestName = "Database has connection string")]
    [TestCase("redis", TestName = "Cache has connection string")]
    public async Task ResourceHasConnectionString(string resourceName)
    {
        var connectionString = await _app.GetConnectionStringAsync(resourceName);
        connectionString.ShouldNotBeNullOrEmpty();
    }
}