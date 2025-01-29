using CraftersCloud.Core.Results;
using CraftersCloud.ReferenceArchitecture.Core.Caching;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using OneOf;

namespace CraftersCloud.ReferenceArchitecture.Application.Identity.GetUserById;

public sealed record Query(UserId Id) : ICachedQuery<OneOf<QueryResponse, NotFoundResult>>
{
    public CachingOptions CachingOptions => CachingOptions.For<Query>(Id.Value.ToString());
    public IEnumerable<string> Tags { get; } = ["users"];
}