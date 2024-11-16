using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.ComplexExamples.Users;

[UsedImplicitly]
public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("complex/users")
            .WithGroupName("Users")
            .RequireAuthorization()
            .RequirePermissions(PermissionId.UsersRead);

        group.MapGet("/", SimpleExamples.Users.GetUsers.Handle);
        group.MapGet("/{id:guid}", SimpleExamples.Users.GetUserDetails.Handle);

        group.MapPut("/", SimpleExamples.Users.UpdateUser.Handle).Validate<SimpleExamples.Users.UpdateUser.Request>()
            .RequirePermissions(PermissionId.UsersWrite);
        group.MapPost("/", SimpleExamples.Users.CreateUser.Handle).Validate<SimpleExamples.Users.CreateUser.Request>()
            .RequirePermissions(PermissionId.UsersWrite);

        group.MapGet("/roles", GetRoles.Handle);
        group.MapGet("/statuses", GetStatuses.Handle);
    }
}