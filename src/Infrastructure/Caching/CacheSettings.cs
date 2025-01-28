namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

[UsedImplicitly]
internal class CacheSettings
{
    public const string SectionName = "App:Cache";
    public TimeSpan DefaultDuration { get; init; } = TimeSpan.FromMinutes(5);
}