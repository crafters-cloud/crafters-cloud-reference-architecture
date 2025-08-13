using CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using GetUserById = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.GetUserById;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class HelloWorldEndpointsFixture : EndpointsFixtureBase
{
    private IFlurlRequest _endpoint;

    [SetUp]
    public void SetUp() => _endpoint = Client.Request("hello-world");

    [Test]
    public async Task HelloWorld()
    {
        var response = await _endpoint
            .GetJsonAsync<HelloWorld.Response>();

        await Verify(response);
    }
}