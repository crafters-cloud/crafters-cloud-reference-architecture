using System.Reflection;
using System.Text.Json;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public static class ServiceCollectionExtensions
{
    public static void AppAddCaching(this IServiceCollection services, IConfiguration configuration,
        Assembly[] extraAssemblies)
    {
        var settings = configuration.GetRequiredSection(CacheSettings.SectionName).Get<CacheSettings>() ??
                       throw new InvalidOperationException("Cache settings are missing");

        var redisConnectionString = configuration.GetConnectionString("Redis") ??
                                    throw new InvalidOperationException("Redis connection string is missing");

        services.AddFusionCache()
            .WithDefaultEntryOptions(options =>
            {
                options.Duration = settings.DefaultLocalCacheExpiration;
                options.DistributedCacheDuration = settings.DefaultExpiration;
            })
            .WithSerializer(new FusionCacheSystemTextJsonSerializer(CrateJsonSerializerOptions(extraAssemblies)))
            .WithDistributedCache(new RedisCache(
                new RedisCacheOptions
                {
                    Configuration = redisConnectionString
                }))
            .AsHybridCache();
    }

    private static JsonSerializerOptions CrateJsonSerializerOptions(Assembly[] extraAssemblies)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = false
        };
        options.Converters.AppRegisterJsonConverters(extraAssemblies);
        return options;
    }
}