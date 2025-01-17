using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Configuration;

public class TestConfigurationBuilder
{
    private string _dbContextName = string.Empty;
    private string _connectionString = string.Empty;
    private readonly Action<ConfigurationBuilder>? _extraConfiguration = null;

    public TestConfigurationBuilder WithDbName(string dbName)
    {
        _dbContextName = dbName;
        return this;
    }

    public TestConfigurationBuilder WithConnectionString(string connectionString)
    {
        _connectionString = connectionString;
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
            { $"ConnectionStrings:{_dbContextName}", _connectionString },
            { "HealthChecks:TokenAuthorizationEnabled", "false" },
            { "Swagger:Enabled", "false" },
            { "KeyVault:Enabled", "false" },
            { "ApplicationInsights:ConnectionString", "" }
        };

        configurationBuilder.AddInMemoryCollection(dict);
        _extraConfiguration?.Invoke(configurationBuilder);
        return configurationBuilder.Build();
    }

    private void EnsureParametersBeforeBuild()
    {
        if (string.IsNullOrWhiteSpace(_dbContextName))
        {
            throw new InvalidOperationException("Missing db context name");
        }
    }
}