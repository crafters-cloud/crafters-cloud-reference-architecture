using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.KeyVault;

public static class ConfigurationBuilderExtensions
{
    public static void AppAddAzureKeyVault(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(KeyVaultSettings.SectionName).Get<KeyVaultSettings>();

        if (settings is { Enabled: true })
        {
            var keyVaultUri = new Uri($"https://{settings.Name}.vault.azure.net");
            builder.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
        }
    }
}