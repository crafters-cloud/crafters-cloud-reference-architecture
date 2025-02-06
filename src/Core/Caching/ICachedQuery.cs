using CraftersCloud.Core.Cqrs;

namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public interface ICachedQuery : IBaseQuery
{
    CachingOptions CachingOptions { get; }
}

public interface ICachedQuery<out TResponse> : ICachedQuery, IQuery<TResponse>;