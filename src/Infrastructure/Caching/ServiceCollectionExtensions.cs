using System.Reflection;
using System.Text.Json;
using CraftersCloud.ReferenceArchitecture.Core.Caching;
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
        Assembly[] jsonConverterAssemblies, Assembly[] cacheEvictorAssemblies)
    {
        var settings = configuration.GetRequiredSection(CacheSettings.SectionName).Get<CacheSettings>() ??
                       throw new InvalidOperationException("Cache settings are missing");

        var redisConnectionString = configuration.GetConnectionString("Redis") ??
                                    throw new InvalidOperationException("Redis connection string is missing");

        services.CoreAddCaching(c =>
        {
            c.DefaultExpiration = settings.DefaultExpiration;
            c.DefaultLocalCacheExpiration = settings.DefaultLocalCacheExpiration;
            c.RedisConnectionString = redisConnectionString;
            c.JsonConvertersAssemblies = jsonConverterAssemblies;
            c.CacheEvictorAssemblies = cacheEvictorAssemblies;
        });
    }

    public static void CoreAddCaching(this IServiceCollection services, Action<CachingOptions> configureCaching)
    {
        var options = new CachingOptions();
        configureCaching(options);

        services.ConfigureCacheEntryOptions(options);
        services.AddSingleton<ICacheEvictorsRegistry>(_ =>
            CreateCacheEvictorsRegistry(options.CacheEvictorAssemblies));
        services.AddHybridCache(options);
    }

    private static void AddHybridCache(this IServiceCollection services, CachingOptions options) =>
        services.AddFusionCache()
            .WithDefaultEntryOptions(fusionCacheEntryOptions =>
            {
                fusionCacheEntryOptions.Duration = options.DefaultLocalCacheExpiration;
                fusionCacheEntryOptions.DistributedCacheDuration = options.DefaultExpiration;
            })
            .WithSerializer(
                new FusionCacheSystemTextJsonSerializer(CrateJsonSerializerOptions(options.JsonConvertersAssemblies)))
            .WithDistributedCache(new RedisCache(
                new RedisCacheOptions
                {
                    Configuration = options.RedisConnectionString
                }))
            .AsHybridCache();

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

    public static void ConfigureCacheEntryOptions(this IServiceCollection services, CachingOptions options) =>
        services.AddCacheEntryConfiguration(Directory.GetCurrentDirectory(), "cacheEntrySettings.json", options);

    public static void AddCacheEntryConfiguration(this IServiceCollection services, string basePath,
        string cacheSettingsFileName, CachingOptions options)
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { nameof(CacheEntryOptions.DefaultLocalCacheExpiration), options.DefaultLocalCacheExpiration.ToString() },
            { nameof(CacheEntryOptions.DefaultExpiration), options.DefaultExpiration.ToString() }
        };

        var cacheConfig = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(cacheSettingsFileName, true, true)
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        services.Configure<CacheEntryOptions>(cacheConfig);
    }

    private static CacheEvictorsRegistry CreateCacheEvictorsRegistry(Assembly[] cacheEvictorsAssemblies)
    {
        var result = new CacheEvictorsRegistry();
        var evictors = new List<CacheEvictor>();

        foreach (var assembly in cacheEvictorsAssemblies)
        {
            var cacheEvictors = assembly.GetTypes().Where(t =>
                typeof(CacheEvictorsBase).IsAssignableFrom(t)
                && !t.IsAbstract);

            foreach (var cacheEvictor in cacheEvictors)
            {
                var instance = (CacheEvictorsBase) Activator.CreateInstance(cacheEvictor)!;
                evictors.AddRange(instance);
            }
        }

        result.Register(evictors);
        return result;
    }

    public class CachingOptions
    {
        public TimeSpan DefaultLocalCacheExpiration { get; set; } = TimeSpan.FromMinutes(5);

        public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(5);
        public string RedisConnectionString { get; set; } = string.Empty;
        public Assembly[] JsonConvertersAssemblies { get; set; } = [];
        public Assembly[] CacheEvictorAssemblies { get; set; } = [];
    }
}