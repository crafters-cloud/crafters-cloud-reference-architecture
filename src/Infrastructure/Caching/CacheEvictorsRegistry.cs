using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CacheEvictorsRegistry : ICacheEvictorsRegistry
{
    private readonly Dictionary<string, List<CacheEvictor>> _evictors = [];

    public void Register(IEnumerable<CacheEvictor> cacheEvictors)
    {
        foreach (var cacheEvictor in cacheEvictors)
        {
            var key = cacheEvictor.CacheEvictorKey;
            if (!_evictors.TryGetValue(key, out var evictors))
            {
                evictors = [];
                _evictors.TryAdd(key, evictors);
            }

            evictors.Add(cacheEvictor);
        }
    }

    public IEnumerable<CacheEvictor<T>> GetEvictors<T>(T notification) where T : INotification
    {
        var key = CacheEvictor<T>.GetCacheEvictorKey();
        var result = _evictors.GetValueOrDefault(key, []).OfType<CacheEvictor<T>>();
        return result;
    }
}