using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

[UsedImplicitly]
public class InvalidateCacheNotificationHandler<T>(HybridCache cache)
    : INotificationHandler<T> where T : ICacheTagEvictor, INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        var tasks = notification.Tags.Select(t => cache.RemoveByTagAsync(t, cancellationToken).AsTask());
        await Task.WhenAll(tasks);
    }
}