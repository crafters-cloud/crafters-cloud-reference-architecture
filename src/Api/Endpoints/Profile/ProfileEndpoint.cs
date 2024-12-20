namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Identity;

[UsedImplicitly]
public class ProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("profile")
            .RequireAuthorization()
            .WithGroupName("Profile");
        
        group.MapGet("/", GetUserProfile.Handle);
    }
}