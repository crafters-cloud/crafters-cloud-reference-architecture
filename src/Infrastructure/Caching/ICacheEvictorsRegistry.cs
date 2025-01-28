using CraftersCloud.ReferenceArchitecture.Core.Caching;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Caching;

public interface ICacheEvictorsRegistry
{
    IEnumerable<CacheEvictor<T>> GetEvictors<T>(T notification) where T : INotification;
}