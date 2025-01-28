using Testcontainers.Redis;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Cache;

public class TestCache
{
    public string ConnectionString { get; private set; } = null!;
    private static RedisContainer? _container;

    public async Task CreateAsync()
    {
        try
        {
            _container ??= new RedisBuilder()
                .WithReuse(true)
                .WithName("crafters-cloud-reference-architecture-redis-integration-tests")
                .WithLabel("reuse-id", "crafters-cloud-reference-architecture-redis-integration-tests")
                .Build();

            await _container!.StartAsync();
            ConnectionString = _container.GetConnectionString();
            WriteLine($"Redis Docker connection string: {ConnectionString}");
        }
        catch (Exception e)
        {
            WriteLine($"Failed to start redis docker container: {e.Message}");
            throw;
        }
    }

    private static void WriteLine(string value) => TestContext.Out.Write(value);
}