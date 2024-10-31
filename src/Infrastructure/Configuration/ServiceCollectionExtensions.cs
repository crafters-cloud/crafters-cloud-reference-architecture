using CraftersCloud.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AppConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DbContextSettings>(DbContextSettings.SectionName, configuration);
        services.Configure<ApplicationInsightsSettings>(ApplicationInsightsSettings.SectionName, configuration);
    }
}