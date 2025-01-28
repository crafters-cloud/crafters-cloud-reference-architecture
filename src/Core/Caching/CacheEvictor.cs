using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public abstract class CacheEvictor(string cacheEvictorKey)
{
    public string CacheEvictorKey { get; } = cacheEvictorKey;
}

public abstract class CacheEvictor<T>(Func<T, string> getCacheKey) : CacheEvictor(GetCacheEvictorKey())
{
    protected readonly Func<T, string> GetCacheKey = getCacheKey;

    public abstract Task RemoveAsync(HybridCache cache, T notification);

    public static string GetCacheEvictorKey() => typeof(T).FullName!;
}

public class CacheEvictor<TCacheRequest, T>(Func<T, string> getCacheKey) : CacheEvictor<T>(getCacheKey)
    where TCacheRequest : ICachedQuery
{
    public override async Task RemoveAsync(HybridCache cache, T notification)
    {
        var key = CachingOptions.CreateFullCachingKey(typeof(TCacheRequest), GetCacheKey(notification));
        await cache.RemoveAsync(key);
    }
}