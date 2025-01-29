using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Configuration;

public class TestConfigurationBuilder
{
    private string _dbName = string.Empty;
    private string _dbConnectionString = string.Empty;
    private string _cacheConnectionString = string.Empty;
    private readonly Action<ConfigurationBuilder>? _extraConfiguration = null;

    public TestConfigurationBuilder WithDbName(string dbName)
    {
        _dbName = dbName;
        return this;
    }

    public TestConfigurationBuilder WithDbConnectionString(string connectionString)
    {
        _dbConnectionString = connectionString;
        return this;
    }
    
    public TestConfigurationBuilder WithCacheConnectionString(string connectionString)
    {
        _cacheConnectionString = connectionString;
        return this;
    }

    public IConfiguration Build()
    {
        EnsureParametersBeforeBuild();
        var configurationBuilder = new ConfigurationBuilder();

        var dict = new Dictionary<string, string?>
        {
            { "UseDeveloperExceptionPage", "true" },
            { "DbContext:SensitiveDataLoggingEnabled", "true" },
            { "DbContext:UseAccessToken", "false" },
            { "DbContext:ConnectionResiliencyMaxRetryCount", "10" },
            { "DbContext:ConnectionResiliencyMaxRetryDelay", "0.00:00:30" },
            { "DbContext:RegisterMigrationsAssembly", "true" },
            { $"ConnectionStrings:{_dbName}", _dbConnectionString },
            { "ConnectionStrings:Redis", _cacheConnectionString },
            { "HealthChecks:TokenAuthorizationEnabled", "false" },
            { "Swagger:Enabled", "false" },
            { "KeyVault:Enabled", "false" },
            { "ApplicationInsights:ConnectionString", "" },
            // by setting the default cache duration to 0, we just pass through cache
            { "App:Cache:DefaultDuration", "0.00:00:00" }
        };

        configurationBuilder.AddInMemoryCollection(dict);
        _extraConfiguration?.Invoke(configurationBuilder);
        return configurationBuilder.Build();
    }

    private void EnsureParametersBeforeBuild()
    {
        if (string.IsNullOrWhiteSpace(_dbName))
        {
            throw new InvalidOperationException("Missing db context name");
        }
    }
}