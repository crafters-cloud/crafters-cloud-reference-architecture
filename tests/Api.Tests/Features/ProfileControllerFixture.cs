using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using GetUserProfile = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Identity.GetUserProfile;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Features;

[Category("integration")]
public class ProfileControllerFixture : IntegrationFixtureBase
{
    [Test]
    public async Task GetUserProfile()
    {
        var profile = await Client.GetAsync<GetUserProfile.Response>("profile");
        await Verify(profile);
    }
}