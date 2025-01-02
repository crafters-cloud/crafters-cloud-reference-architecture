using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

[UsedImplicitly]
public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users")
            .WithGroupName("Users")
            .RequireAuthorization()
            .RequirePermissions(PermissionId.UsersRead);

        group.MapGet("/", GetUsers.Handle);
        group.MapGet("/{id:guid}", GetUserDetails.Handle);

        group.MapPut("/", UpdateUser.Handle).Validate<UpdateUser.Request>()
            .RequirePermissions(PermissionId.UsersWrite);
        group.MapPost("/", CreateUser.Handle).Validate<CreateUser.Request>()
            .RequirePermissions(PermissionId.UsersWrite);

        group.MapGet("/roles", GetRoles.Handle);
        group.MapGet("/statuses", GetStatuses.Handle);
    }
}