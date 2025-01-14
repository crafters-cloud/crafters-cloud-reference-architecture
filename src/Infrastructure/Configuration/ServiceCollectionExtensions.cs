using CraftersCloud.Core.EntityFramework.Infrastructure;
using CraftersCloud.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AppConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DbContextSettings>()
            .Bind(configuration.GetSection(DbContextSettings.SectionName))
            .ValidateDataAnnotations();
        services.AddOptions<ApplicationInsightsSettings>()
            .Bind(configuration.GetSection(ApplicationInsightsSettings.SectionName))
            .ValidateDataAnnotations();
    }
}