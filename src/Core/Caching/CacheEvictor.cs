namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public abstract class CacheEvictor(string cacheEvictorKey)
{
    public string CacheEvictorKey { get; } = cacheEvictorKey;
}

public class CacheEvictor<T>(Func<T, string> getCacheKey) : CacheEvictor(GetCacheEvictorKey())
{
    public readonly Func<T, string> GetCacheKey = getCacheKey;

    public static string GetCacheEvictorKey() => typeof(T).FullName!.RemoveCharactersNotSuitableForCacheKey();
}