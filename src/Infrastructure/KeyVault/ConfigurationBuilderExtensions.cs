using Azure.Identity;
using CraftersCloud.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.KeyVault;

public static class ConfigurationBuilderExtensions
{
    public static void AppAddAzureKeyVault(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        var settings = configuration.GetOptions<KeyVaultSettings>(KeyVaultSettings.SectionName);

        if (settings is { Enabled: true })
        {
            var keyVaultUri = new Uri($"https://{settings.Name}.vault.azure.net");
            builder.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
        }
    }
}