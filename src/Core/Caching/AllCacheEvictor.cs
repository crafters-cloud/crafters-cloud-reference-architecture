using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public class AllCacheEvictor<TCacheRequest, T>() : CacheEvictor<T>(_ => string.Empty)
    where TCacheRequest : ICachedQuery
{
    public override async Task RemoveAsync(HybridCache cache,
        T notification)
    {
        var pattern = CachingOptions.CreateRemoveAllCachingKeyPattern<TCacheRequest>();
        await cache.RemoveByTagAsync(pattern);
    }
}