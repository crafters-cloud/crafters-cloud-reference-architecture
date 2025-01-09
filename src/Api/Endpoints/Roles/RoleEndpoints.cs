using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Roles;

[UsedImplicitly]
public class RoleEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("roles")
            .WithGroupName("Roles")
            .RequireAuthorization()
            .RequirePermissions(PermissionId.RolesRead);

        group.MapGet("/", GetRoles.Handle);
    }
}