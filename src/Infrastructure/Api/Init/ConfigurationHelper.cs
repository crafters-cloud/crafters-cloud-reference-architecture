using CraftersCloud.ReferenceArchitecture.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class ConfigurationHelper
{
    // needed because of early reading of configuration file in Program.cs (e.g. for Serilog or KeyVault), before WebHostBuilder has been built.
    // Later new configuration is built again.
    public static IConfiguration CreateBoostrapConfiguration(string environmentParameterPrefix = "ASPNETCORE_")
    {
        var environment = Environment.GetEnvironmentVariable($"{environmentParameterPrefix}ENVIRONMENT");
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile(
                $"appsettings.{environment ?? "Production"}.json",
                true)
            .AddTestConfiguration()
            .Build();
    }
}