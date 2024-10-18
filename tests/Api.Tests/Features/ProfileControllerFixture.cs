using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.ReferenceArchitecture.Api.Features.Authorization;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Features;

[Category("integration")]
public class ProfileControllerFixture : IntegrationFixtureBase
{
    [Test]
    public async Task GetUserProfile()
    {
        var profile = await Client.GetAsync<GetUserProfile.Response>("api/profile");
        await Verify(profile);
    }
}