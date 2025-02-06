using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public class CachingPipelineBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingPipelineBehavior<TRequest, TResponse>> logger,
    IOptions<CacheEntryOptions> cacheEntryOptions)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var (fullCachingKey, cacheEntryOptionKey) = request.CachingOptions;

        var cacheEntryOption = GetCacheEntryOption(cacheEntryOptionKey);
        var tags = request.CachingOptions.Tags;

        return await cache.GetOrCreateAsync<TResponse>(fullCachingKey, async _ =>
        {
            logger.LogDebug("Object not found in cache by {Key}. Continuing the pipeline execution to get the object",
                fullCachingKey);
            return await next();
        }, new HybridCacheEntryOptions
        {
            LocalCacheExpiration = cacheEntryOption.LocalCacheExpiration,
            Expiration = cacheEntryOption.Expiration
        }, cancellationToken: cancellationToken, tags: tags);
    }

    private CacheEntryOption GetCacheEntryOption(string cacheEntryOptionKey)
    {
        var cacheEntryFound = cacheEntryOptions.Value.TryFindEntry(cacheEntryOptionKey, out var cacheEntryOption);
        if (!cacheEntryFound)
        {
            logger.LogWarning(
                "Cache entry option not found for key {Key}. Using the default CacheEntryOption. If caching is not needed remove the marker interface from the request.",
                cacheEntryOptionKey);
        }

        return cacheEntryOption;
    }
}