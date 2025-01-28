using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public static class ServiceCollectionExtensions
{
    public static void AppAddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(CacheSettings.SectionName).Get<CacheSettings>() ??
                       throw new InvalidOperationException("Cache settings are missing");

        var redisConnectionString = configuration.GetConnectionString("Redis") ??
                                    throw new InvalidOperationException("Redis connection string is missing");

        services.AddFusionCache()
            .WithDefaultEntryOptions(options => options.Duration = settings.DefaultDuration)
            .WithSerializer(new FusionCacheSystemTextJsonSerializer())
            .WithDistributedCache(new RedisCache(
                new RedisCacheOptions
                {
                    Configuration = redisConnectionString
                }))
            .AsHybridCache();
    }
}