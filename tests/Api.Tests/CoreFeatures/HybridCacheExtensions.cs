using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

internal static class HybridCacheExtensions
{
    /// <summary>
    /// Returns true if the cache contains an item with a matching key.
    /// </summary>
    /// <param name="cache">An instance of <see cref="HybridCache"/></param>
    /// <param name="key">The name (key) of the item to search for in the cache.</param>
    /// <returns>True if the item exists already. False if it doesn't.</returns>
    /// <remarks>Will never add or alter the state of any items in the cache.</remarks>
    public static async Task<bool> ExistsAsync(this HybridCache cache, string key)
    {
        var (exists, _) = await TryGetValueAsync<object>(cache, key);
        return exists;
    }

    /// <summary>
    /// Returns true if the cache contains an item with a matching key, along with the value of the matching cache entry.
    /// </summary>
    /// <typeparam name="T">The type of the value of the item in the cache.</typeparam>
    /// <param name="cache">An instance of <see cref="HybridCache"/></param>
    /// <param name="key">The name (key) of the item to search for in the cache.</param>
    /// <returns>A tuple of <see cref="bool"/> and the object (if found) retrieved from the cache.</returns>
    /// <remarks>Will never add or alter the state of any items in the cache.</remarks>
    public static async Task<(bool, T?)> TryGetValueAsync<T>(this HybridCache cache, string key)
    {
        var result = await cache.GetOrCreateAsync<object, object?>(
            key,
            null!,
            DoNothing,
            Options,
            null,
            CancellationToken.None);

        return (result is not null, (T)result!);
    }

    /// <summary>
    /// These flags stop the extension methods from adding or modifying any items in the cache.
    /// </summary>
    /// <remarks>
    /// Need to declare these options as internal so that unit tests can find and use them.
    /// </remarks>
    internal static readonly HybridCacheEntryOptions Options = new()
    {
        Flags = HybridCacheEntryFlags.DisableUnderlyingData | HybridCacheEntryFlags.DisableLocalCacheWrite | HybridCacheEntryFlags.DisableDistributedCacheWrite
    };

    /// <summary>
    /// Does nothing.
    /// </summary>
    /// <remarks>Need to declare this as internal so that unit tests can pass it to a mock of the <see cref="HybridCache"/>.</remarks>
    internal static async ValueTask<object?> DoNothing(object _, CancellationToken __) => await ValueTask.FromResult<object?>(null);
}