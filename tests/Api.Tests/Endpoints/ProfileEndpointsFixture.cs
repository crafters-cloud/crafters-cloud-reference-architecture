using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using Flurl.Http;
using GetUserProfile = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Profile.GetUserProfile;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class ProfileEndpointsFixture : IntegrationFixtureBase
{
    [Test]
    public async Task GetUserProfile()
    {
        var profile = await Client.Request("profile").GetJsonAsync<GetUserProfile.Response>();
        await Verify(profile);
    }
}