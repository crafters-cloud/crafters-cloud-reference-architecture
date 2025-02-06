using CraftersCloud.Core.Results;
using CraftersCloud.ReferenceArchitecture.Core.Caching;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.DomainEvents;
using OneOf;

namespace CraftersCloud.ReferenceArchitecture.Application.Identity.GetUserById;

public sealed record Query(UserId Id) : ICachedQuery<OneOf<QueryResponse, NotFoundResult>>
{
    public CachingOptions CachingOptions => CachingOptions
        .For<Query>(CreateKey(Id))
        .WithTags(UserCacheTags.Users, UserCacheTags.User(Id));

    private static string CreateKey(UserId id) => id.ToString();

    [UsedImplicitly]
    internal class CacheEvictors : CacheEvictorsBase
    {
        public CacheEvictors() => EvictOn<UserUpdatedDomainEvent>(c => CreateKey(c.Id));
    }
}