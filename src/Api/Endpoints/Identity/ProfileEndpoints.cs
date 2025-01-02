namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Identity;

[UsedImplicitly]
public class ProfileEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("profile")
            .RequireAuthorization()
            .WithGroupName("Profile");

        group.MapGet("/", GetUserProfile.Handle);
    }
}