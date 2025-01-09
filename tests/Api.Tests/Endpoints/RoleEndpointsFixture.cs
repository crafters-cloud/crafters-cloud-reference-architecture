using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Roles;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using Flurl.Http;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class RoleEndpointsFixture : IntegrationFixtureBase
{
    [Test]
    public async Task GetRoles()
    {
        var response = await Client.Request("roles").GetJsonAsync<GetRoles.Response>();
        await Verify(response);
    }
}