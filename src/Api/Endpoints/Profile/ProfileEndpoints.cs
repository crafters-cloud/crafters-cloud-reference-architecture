namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Profile;

[UsedImplicitly]
public class ProfileEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("profile")
            .WithGroupName("Profile");

        group.MapGet("/", GetUserProfile.Handle).RequireAuthorization();
        group.MapGet("/hello-world", GetUserProfile.HandleHelloWorld);
    }
}