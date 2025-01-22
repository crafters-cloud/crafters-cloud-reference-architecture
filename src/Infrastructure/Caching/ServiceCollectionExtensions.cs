using System.Reflection;
using CraftersCloud.Core.Caching;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public static class ServiceCollectionExtensions
{
    public static void AppAddCaching(this IServiceCollection services, IConfiguration configuration,
        Assembly entryAssembly)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis") ??
                                    throw new InvalidOperationException("Redis connection string is missing");

        services.AddCoreCaching(cachingOptions =>
        {
            cachingOptions.RedisConnectionString = redisConnectionString;
            cachingOptions.ConfigureJsonSerializerOptions = jsonSerializerOptions =>
                jsonSerializerOptions.Converters.AppRegisterJsonConverters(entryAssembly);
            
            cachingOptions.CacheSettingsFileBasePath = Directory.GetCurrentDirectory();
            cachingOptions.CacheSettingsFileName = "cacheSettings.json";
        });
    }
}