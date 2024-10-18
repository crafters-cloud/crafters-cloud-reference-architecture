using Azure.Identity;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Init;

public static class ConfigurationBuilderExtensions
{
    public static void AppAddAzureKeyVault(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        var settings = configuration.ReadKeyVaultSettings();

        if (settings is { Enabled: true })
        {
            var keyVaultUri = new Uri($"https://{settings.Name}.vault.azure.net");
            builder.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
        }
    }
}