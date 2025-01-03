using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Profile;

public static partial class GetUserProfile
{
    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public IReadOnlyCollection<PermissionId> Permissions { get; set; } = [];
    }

    [Mapper]
    public static partial class ResponseMapper
    {
        [UserMapping(Default = true)]
        public static Response ToResponse(User source)
        {
            var dto = MapToResponse(source);
            dto.Permissions = source.GetPermissionIds();
            return dto;
        }

        [MapperIgnoreTarget(nameof(Response.Permissions))]
        private static partial Response MapToResponse(User source);
    }

    public static async Task<Results<Ok<Response>, NotFound>> Handle(ICurrentUserProvider currentUserProvider,
        IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        if (currentUserProvider.UserId is null)
        {
            return TypedResults.NotFound();
        }

        var user = await repository.QueryAll().QueryById(currentUserProvider.UserId.Value)
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .Include(u => u.UserStatus)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        return user.ToMappedMinimalApiResult(ResponseMapper.ToResponse);
    }
}