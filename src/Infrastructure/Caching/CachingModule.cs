using System.Reflection;
using Autofac;
using CraftersCloud.ReferenceArchitecture.Core.Caching;
using Microsoft.Extensions.Logging;
using Module = Autofac.Module;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CachingModule : Module
{
    public Assembly[] CacheEvictorsAssemblies { get; init; } = [];

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(CreateCacheEvictorsRegistry).AsImplementedInterfaces().SingleInstance();
        builder.Register(ResolveCacheEntriesSettings).As<CacheSettingsEntries>().SingleInstance();
    }

    private static CacheSettingsEntries ResolveCacheEntriesSettings(IComponentContext container)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "cachesettings.json");
        var entries = CacheSettingsEntries.ReadEntries(path);
        return new CacheSettingsEntries(entries, container.Resolve<ILogger<CacheSettingsEntries>>());
    }

    private CacheEvictorsRegistry CreateCacheEvictorsRegistry(IComponentContext context)
    {
        var result = new CacheEvictorsRegistry();
        var evictors = new List<CacheEvictor>();

        foreach (var assembly in CacheEvictorsAssemblies)
        {
            var cacheEvictors = assembly.GetTypes().Where(t =>
                t.IsAssignableTo<CacheEvictorsBase>()
                && !t.IsAbstract);

            foreach (var cacheEvictor in cacheEvictors)
            {
                var instance = (CacheEvictorsBase) Activator.CreateInstance(cacheEvictor)!;
                evictors.AddRange(instance);
            }
        }

        result.Register(evictors);
        return result;
    }
}