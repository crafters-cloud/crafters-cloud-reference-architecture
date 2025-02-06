namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public record CachingOptions(string FullCachingKey, string CacheEntryOptionKey)
{
    private IList<string>? _tags;
    public IEnumerable<string>? Tags => _tags?.AsReadOnly();

    public static CachingOptions For<T>(string cacheKeyPart) where T : ICachedQuery =>
        new CachingOptions<T>(cacheKeyPart);

    public CachingOptions WithTags(params string[] tags)
    {
        _tags ??= [];
        foreach (var tag in tags)
        {
            _tags.Add(tag);
        }

        return this;
    }
}

public record CachingOptions<T> : CachingOptions where T : ICachedQuery
{
    internal CachingOptions(string cacheKeyPart) : base(CacheKeyHelper.CreateFullCachingKey<T>(cacheKeyPart),
        CacheKeyHelper.CreateCacheEntryOptionKey<T>())
    {
    }
}