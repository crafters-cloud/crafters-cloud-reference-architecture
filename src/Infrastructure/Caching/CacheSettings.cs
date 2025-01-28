namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

[UsedImplicitly]
public class CacheSettings
{
    public const string SectionName = "App:Cache";
    public TimeSpan DefaultLocalCacheExpiration { get; init; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultExpiration { get; init; } = TimeSpan.FromMinutes(5);
}