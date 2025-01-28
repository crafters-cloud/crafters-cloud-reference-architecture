using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

[UsedImplicitly]
public class InvalidateCacheNotificationHandler<T>(ICacheEvictorsRegistry cacheEvictorsRegistry, HybridCache cache)
    : INotificationHandler<T> where T : INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        var cacheEvictors = cacheEvictorsRegistry.GetEvictors(notification);
        var removeTasks = cacheEvictors.Select(evictor => evictor.RemoveAsync(cache, notification));
        await Task.WhenAll(removeTasks);
    }
}