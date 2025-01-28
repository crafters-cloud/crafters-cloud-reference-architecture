using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CacheSettingsEntries(IEnumerable<CacheSettingEntry> entries, ILogger<CacheSettingsEntries> logger)
{
    private readonly Dictionary<string, CacheSettingEntry> _settings = entries.ToDictionary(x => x.Key, x => x);
    public IEnumerable<CacheSettingEntry> Entries => _settings.Values;

    public static IEnumerable<CacheSettingEntry> ReadEntries(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File was not found: {path}");
        }

        var fileContent = File.ReadAllText(path);
        var values = JsonSerializer.Deserialize<CacheSettingEntry[]>(fileContent) ??
                     throw new InvalidOperationException("Could not deserialize cache settings");
        return values;
    }

    public CacheSettingEntry? FindExpirationFor(string key)
    {
        if (_settings.TryGetValue(key, out var value))
        {
            logger.LogDebug("Found cache entry: {Key}, {Expiration}, {LocalCacheExpiration}", key, value.Expiration,
                value.LocalCacheExpiration);
            return value;
        }
        
        logger.LogWarning("Could not find cache timeout for {Key}.", key);
        return null;
    }
}