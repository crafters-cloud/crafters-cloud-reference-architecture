namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public interface ICacheTagEvictor
{
    public string[] Tags { get; }
}