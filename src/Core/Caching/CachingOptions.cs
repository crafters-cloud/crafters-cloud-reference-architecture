using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public record CachingOptions(string FullCachingKey, string CacheSettingEntryKey)
{
    public static string CreateRemoveAllCachingKeyPattern<T>() where T : ICachedQuery =>
        CreateFullCachingKey<T>("*");

    protected static string CreateFullCachingKey<T>(string key) where T : ICachedQuery =>
        CreateFullCachingKey(typeof(T), key);

    public static string CreateFullCachingKey(Type type, string key)
    {
        var result = RemoveNonRelevantNamespaces(type)
            .RemoveCharactersNotSuitableForCacheKey() + ":" + key;

        return result;
    }

    protected static string CreateCacheEntrySettingKey<T>() where T : ICachedQuery =>
        CreateCacheEntrySettingKey(typeof(T));

    public static string CreateCacheEntrySettingKey(Type type)
    {
        var result = RemoveNonRelevantNamespaces(type)
            .RemoveCharactersNotSuitableForCacheKey()
            .RemoveEverythingAfterInclusive('+');

        return result;
    }

    private static string RemoveNonRelevantNamespaces(Type type) =>
        type.FullName?.Replace("CraftersCloud.ReferenceArchitecture.", "") ?? string.Empty;
    
    public static CachingOptions For<T>(string key) where T : ICachedQuery =>
        new CachingOptions<T>(key);
}

public record CachingOptions<T> : CachingOptions where T : ICachedQuery
{
    internal CachingOptions(string key) : base(CreateFullCachingKey<T>(key), CreateCacheEntrySettingKey<T>())
    {
    }
}