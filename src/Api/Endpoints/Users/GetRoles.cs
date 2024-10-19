using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Api.Features;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class GetRoles
{
    [UsedImplicitly]
    public class Endpoint(IRepository<Role> roleRepository) : EndpointWithoutRequest<IEnumerable<LookupResponse<Guid>>>
    {
        // TODO solve this   [UserHasPermission(PermissionId.UsersRead)]
        public override void Configure() => Get("/api/users/roles");

        public override async Task<IEnumerable<LookupResponse<Guid>>> ExecuteAsync(CancellationToken cancellationToken) =>
            await roleRepository
                .QueryAll()
                .Select(r => new LookupResponse<Guid> { Value = r.Id, Label = r.Name })
                .OrderBy(r => r.Label)
                .ToListAsync(cancellationToken);
    }
}