using System.Collections;

namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public abstract class CacheEvictorsBase : IEnumerable<CacheEvictor>
{
    private readonly List<CacheEvictor> _evictors = [];

    protected void EvictOn<T>(Func<T, string> func) =>
        _evictors.Add(new CacheEvictor<T>(func));

    public IEnumerator<CacheEvictor> GetEnumerator() => _evictors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}