namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.ComplexExamples.Identity;

[UsedImplicitly]

public class ProfileEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("complex/profile")
            .RequireAuthorization()
            .WithGroupName("Profile");
        
        group.MapGet("/", SimpleExamples.Identity.GetUserProfile.Handle);
    }
}