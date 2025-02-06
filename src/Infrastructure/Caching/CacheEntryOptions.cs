namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CacheEntryOptions
{
    public TimeSpan DefaultLocalCacheExpiration { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
    private Dictionary<string, CacheEntryOption> _entries = [];

    public CacheEntryOption[] Entries
    {
        get => [.. _entries.Values];
        set => _entries = value.ToDictionary(x => x.Key, x => x);
    }
    
    public bool TryFindEntry(string key, out CacheEntryOption entry)
    {
        if (_entries.TryGetValue(key, out var value))
        {
            entry = value;
            return true;
        }
        
        entry = DefaultCacheEntryOption(key);
        return false;
    }

    private CacheEntryOption DefaultCacheEntryOption(string key) =>
        new() { Key = key, LocalCacheExpiration = DefaultLocalCacheExpiration, Expiration = DefaultExpiration };
}