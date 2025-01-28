using System.Reflection;
using CraftersCloud.ReferenceArchitecture.Core.Caching;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.MediatR;
using Microsoft.Extensions.Logging.Abstractions;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Cache;

public class CommonCacheSettingsFixture
{
    private const string SettingsFileName = "cachesettings.json";
    private readonly string[] _assemblyNames;
    private readonly CacheSettingsEntries _settingsEntries;

    public CommonCacheSettingsFixture(string[] assemblyNames)
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Caching", SettingsFileName);
        var cacheSettingEntries = CacheSettingsEntries.ReadEntries(path);
        _settingsEntries = new CacheSettingsEntries(cacheSettingEntries, NullLogger<CacheSettingsEntries>.Instance);

        _assemblyNames = assemblyNames;
    }

    public void TestAllCachedRequestHandlersHaveCorrespondingEntry()
    {
        var requestAndRequestHandlers =
            FindAllRequestsAndRequestHandlersThatAreCached(_assemblyNames);

        var timeouts =
            GetAllTimeouts(requestAndRequestHandlers);

        foreach (var (key, found, meta) in timeouts)
        {
            found.ShouldBeTrue(
                $"{SettingsFileName} does not have entry defined for {meta.RequestType.FullName}, key: {key}");
        }
    }

    private IEnumerable<(string key, bool found, RequestHandlerMeta meta)> GetAllTimeouts(
        IEnumerable<RequestHandlerMeta> requestAndRequestHandlers)
    {
        foreach (var meta in requestAndRequestHandlers)
        {
            if (meta.CachedRequestType == null)
            {
                yield return ("", false, meta);
            }
            else
            {
                var key = CachingOptions.CreateCacheEntrySettingKey(meta.CachedRequestType);
                var entry = _settingsEntries.FindExpirationFor(key);
                yield return (key, entry != null, meta);
            }
        }
    }

    public void TestAllEntriesHaveCorrespondingCacheRequest()
    {
        IEnumerable<RequestHandlerMeta> requestAndRequestHandlers =
            FindAllRequestsAndRequestHandlersThatAreCached(_assemblyNames).ToList();

        var requests = new List<(string key, RequestHandlerMeta? meta)>();
        foreach (var entry in _settingsEntries.Entries)
        {
            var request =
                requestAndRequestHandlers.SingleOrDefault(r =>
                    r.CachedRequestType != null &&
                    CachingOptions.CreateCacheEntrySettingKey(r.CachedRequestType) == entry.Key);
            requests.Add((entry.Key, request));
        }

        // for convenience when troubleshooting first print all that don't have meta
        foreach (var (key, meta) in requests)
        {
            if (meta == null)
            {
                Console.WriteLine($"Request is not found: {key}");
            }
        }

        foreach (var (key, meta) in requests)
        {
            meta.ShouldNotBeNull($"Request: {key} is not found");
            meta.RequestType.ShouldNotBeNull($"{SettingsFileName} does not have entry defined for {key}");
        }
    }

    private static IEnumerable<RequestHandlerMeta>
        FindAllRequestsAndRequestHandlersThatAreCached(string[] assemblyNames) =>
        assemblyNames.SelectMany(assemblyName =>
        {
            var assembly = Assembly.Load(assemblyName);
            var requestAndRequestHandlers =
                assembly.FindAllRequestsAndRequestHandlersThatAreCached();
            return requestAndRequestHandlers;
        });
}