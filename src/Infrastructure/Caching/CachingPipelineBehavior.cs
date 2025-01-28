using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CachingPipelineBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingPipelineBehavior<TRequest, TResponse>> logger,
    CacheSettingsEntries cacheEntriesSettings,
    IOptions<CacheSettings> cacheSettings)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var (fullCachingKey, cacheSettingEntryKey) = request.CachingOptions;
        var cacheSettingEntry = cacheEntriesSettings.FindExpirationFor(cacheSettingEntryKey);
        var tags = request.Tags;

        return await cache.GetOrCreateAsync<TResponse>(fullCachingKey, async _ =>
        {
            logger.LogDebug("Object not found in cache by {Key}. Continuing the pipeline execution", fullCachingKey);
            return await next();
        }, new HybridCacheEntryOptions
        {
            LocalCacheExpiration = cacheSettingEntry?.LocalCacheExpiration ??
                                   cacheSettings.Value.DefaultLocalCacheExpiration,
            Expiration = cacheSettingEntry?.Expiration ??
                         cacheSettings.Value.DefaultExpiration
        }, cancellationToken: cancellationToken, tags:tags);
    }
}