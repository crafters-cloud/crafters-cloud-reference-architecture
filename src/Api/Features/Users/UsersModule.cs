using Carter;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

[UsedImplicitly]
public class UsersModule : CarterModule
{
    public UsersModule() => RequireAuthorization();

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").WithGroupName("Users");

        group.MapGet("/", GetUsers.Handle);
        group.MapGet("/{id:guid}", GetUserDetails.Handle);
        //group.MapPut<UpdateUser.Request>("/", UpdateUser.Handle);
        // group.MapPost<CreateUser.Response>("/", CreateUser.Handle);
        
        group.MapGet("/roles", GetRoles.Handle);
        group.MapGet("/statuses", GetStatuses.Handle);
    }
}