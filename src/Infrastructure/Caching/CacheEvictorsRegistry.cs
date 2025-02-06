using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CacheEvictorsRegistry : ICacheEvictorsRegistry
{
    private Dictionary<string, List<CacheEvictor>> _evictors = [];

    public void Register(IEnumerable<CacheEvictor> cacheEvictors) =>
        _evictors = cacheEvictors.GroupBy(x => x.CacheEvictorKey)
            .ToDictionary(x => x.Key, x => x.ToList());

    public IEnumerable<CacheEvictor<T>> GetEvictors<T>(T notification) where T : INotification
    {
        var key = CacheEvictor<T>.GetCacheEvictorKey();
        var result = _evictors.GetValueOrDefault(key, []).OfType<CacheEvictor<T>>();
        return result;
    }
}