using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;

public static partial class GetUserById
{
    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;
        public string UserStatusName { get; set; } = string.Empty;
        public string UserStatusDescription { get; set; } = string.Empty;
    }

    [Mapper]
    public static partial class ResponseMapper
    {
        public static partial Response? ToResponse(User? source);
    }

    public static async Task<Results<Ok<Response>, NotFound>> Handle(UserId id, IRepository<User> repository,
        HybridCache cache,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{id}";

        var response = await cache.GetOrCreateAsync<Response?>(cacheKey, async _ =>
            await GetResponse(id, repository, cancellationToken), cancellationToken: cancellationToken);

        return response.ToMinimalApiResult();
    }

    private static async Task<Response?> GetResponse(UserId id, IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        var entity = await repository.QueryAll()
            .Include(x => x.UserStatus)
            .AsNoTracking()
            .QueryById(id)
            .QueryActiveOnly()
            .SingleOrDefaultAsync(cancellationToken);

        var response = ResponseMapper.ToResponse(entity);
        return response;
    }
}