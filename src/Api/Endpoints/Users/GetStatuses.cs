using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Api.Features;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class GetStatuses
{
    [UsedImplicitly]
    public class Endpoint(IRepository<UserStatus, UserStatusId> userStatusRepository) : EndpointWithoutRequest<IEnumerable<LookupResponse<UserStatusId>>>
    {
        // TODO solve this   [UserHasPermission(PermissionId.UsersRead)]
        public override void Configure() => Get("/api/users/statuses");

        public override async Task<IEnumerable<LookupResponse<UserStatusId>>> ExecuteAsync(CancellationToken cancellationToken) =>
            await userStatusRepository
                .QueryAll()
                .Select(r => new LookupResponse<UserStatusId> { Value = r.Id, Label = r.Name })
                .OrderBy(r => r.Label)
                .ToListAsync(cancellationToken);
    }
}