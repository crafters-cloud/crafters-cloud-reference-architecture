using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class GetUserDetails
{
    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
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

    public static async Task<Results<Ok<Response>, NotFound>> Handle(Guid id, IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        var user = await repository.QueryAll()
            .Include(x => x.UserStatus)
            .AsNoTracking()
            .QueryById(id)
            .SingleOrDefaultAsync(cancellationToken);

        return user.ToMappedMinimalApiResult(ResponseMapper.ToResponse);
    }
}