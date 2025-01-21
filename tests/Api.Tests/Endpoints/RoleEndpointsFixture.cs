using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Roles;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class RoleEndpointsFixture : EndpointsFixtureBase
{
    [Test]
    public async Task GetRoles()
    {
        var response = await Client.Request("roles").GetJsonAsync<GetRoles.Response>();
        await Verify(response);
    }
}