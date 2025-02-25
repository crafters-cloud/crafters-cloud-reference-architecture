using CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class HelloWorldEndpointsFixture : EndpointsFixtureBase
{
    private IFlurlRequest Endpoint => Client.Request("hello-world");
    
    [Test]
    public async Task HelloWorld()
    {
        var response = await Endpoint
            .GetJsonAsync<HelloWorld.Response>();

        await Verify(response);
    }
}