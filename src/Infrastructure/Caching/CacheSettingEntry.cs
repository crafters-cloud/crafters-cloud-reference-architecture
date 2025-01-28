namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class CacheSettingEntry
{
    public string Key { get; init; } = string.Empty;
    public TimeSpan Expiration { get; init; }
    public TimeSpan LocalCacheExpiration { get; init; }
}