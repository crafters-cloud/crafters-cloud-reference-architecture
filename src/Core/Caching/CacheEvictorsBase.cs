using System.Collections;

namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public abstract class CacheEvictorsBase : IEnumerable<CacheEvictor>
{
    private readonly List<CacheEvictor> _evictors = [];

    protected void Evict<TCacheRequest, T>(Func<T, string> func) where TCacheRequest : ICachedQuery =>
        _evictors.Add(new CacheEvictor<TCacheRequest, T>(func));

    protected void EvictAll<TCacheRequest, T>() where TCacheRequest : ICachedQuery =>
        _evictors.Add(new AllCacheEvictor<TCacheRequest, T>());

    public IEnumerator<CacheEvictor> GetEnumerator() => _evictors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}