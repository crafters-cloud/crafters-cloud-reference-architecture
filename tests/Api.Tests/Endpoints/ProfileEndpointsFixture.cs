using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Profile;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class ProfileEndpointsFixture : EndpointsFixtureBase
{
    [Test]
    public async Task GetUserProfile()
    {
        var profile = await Client.Request("profile").GetJsonAsync<GetUserProfile.Response>();
        await Verify(profile);
    }
}