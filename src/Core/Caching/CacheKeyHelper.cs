namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public static class CacheKeyHelper
{
    public static string CreateFullCachingKey<T>(string cacheKeyPart) where T : ICachedQuery =>
        CreateFullCachingKey(typeof(T), cacheKeyPart);

    public static string CreateFullCachingKey(Type type, string cacheKeyPart)
    {
        var cacheKey = RemoveNonRelevantNamespaces(type)
            .RemoveCharactersNotSuitableForCacheKey();

        return CreateFullCachingKey(cacheKey, cacheKeyPart);
    }

    public static string CreateFullCachingKey(string cacheKey, string cacheKeyPart) => cacheKey + ":" + cacheKeyPart;

    public static string CreateCacheEntryOptionKey<T>() where T : ICachedQuery =>
        CreateCacheEntryOptionKey(typeof(T));

    public static string CreateCacheEntryOptionKey(Type type)
    {
        var result = RemoveNonRelevantNamespaces(type)
            .RemoveCharactersNotSuitableForCacheKey()
            .RemoveEverythingAfterInclusive('+');

        return result;
    }

    private static string RemoveNonRelevantNamespaces(Type type) =>
        type.FullName?.Replace("CraftersCloud.ReferenceArchitecture.", "") ?? string.Empty;
}