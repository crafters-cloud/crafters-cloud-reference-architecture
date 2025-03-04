﻿using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Profile;

public static partial class GetUserProfile
{
    [PublicAPI]
    public class Response
    {
        public UserId Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public IReadOnlyCollection<PermissionId> Permissions { get; set; } = [];
    }

    [Mapper]
    public static partial class Mapper
    {
        [UserMapping(Default = true)]
        public static Response Map(User source)
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

        var entity = await repository.QueryAll().QueryById(currentUserProvider.UserId.Value)
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .Include(u => u.UserStatus)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        return entity.ToMinimalApiResult(Mapper.Map);
    }
}