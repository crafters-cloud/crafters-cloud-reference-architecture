namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.SimpleExamples.Identity;

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