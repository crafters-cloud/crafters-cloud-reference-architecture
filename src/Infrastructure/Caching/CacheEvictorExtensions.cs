using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public static class CacheEvictorExtensions
{
    public static ValueTask RemoveAsync<TNotification>(this CacheEvictor<TNotification> cacheEvictor, HybridCache cache, TNotification notification)
    where TNotification : INotification
    {
        var key = CacheKeyHelper.CreateFullCachingKey(cacheEvictor.CacheEvictorKey, cacheEvictor.GetCacheKey(notification));
        return cache.RemoveAsync(key);
    }
}