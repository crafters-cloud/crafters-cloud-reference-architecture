namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Identity;

[UsedImplicitly]
public class ProfileModule : CarterModule
{
    public ProfileModule() => RequireAuthorization();

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("profile").WithGroupName("Profile");
        group.MapGet("/", GetUserProfile.Handle);
    }
}