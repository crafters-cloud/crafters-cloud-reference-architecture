using Microsoft.Extensions.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    public static bool AppUseDeveloperExceptionPage(this IConfiguration configuration) =>
        configuration.GetValue("UseDeveloperExceptionPage", false);
}