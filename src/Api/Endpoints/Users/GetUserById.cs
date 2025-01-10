using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

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
        public static partial Response ToResponse(User source);
    }

    public static async Task<Results<Ok<Response>, NotFound>> Handle(UserId id, IRepository<User, UserId> repository,
        CancellationToken cancellationToken)
    {
        var entity = await repository.QueryAll()
            .Include(x => x.UserStatus)
            .AsNoTracking()
            .QueryById(id)
            .QueryActiveOnly()
            .SingleOrDefaultAsync(cancellationToken);

        return entity.ToMappedMinimalApiResult(ResponseMapper.ToResponse);
    }
}