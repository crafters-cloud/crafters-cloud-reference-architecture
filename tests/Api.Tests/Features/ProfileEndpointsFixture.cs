using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Identity;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Features;

[Category("integration")]
public class ProfileEndpointsFixture : IntegrationFixtureBase
{
    [Test]
    public async Task GetUserProfile()
    {
        var profile = await Client.GetAsync<GetUserProfile.Response>("profile");
        await Verify(profile);
    }
}