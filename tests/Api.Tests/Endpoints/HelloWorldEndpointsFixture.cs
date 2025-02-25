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
            .AppendQueryParam("userId", "A18C0A9D-A0AD-4D29-9187-B93A5A214AB8")
            .AppendQueryParam("lineItems", """[{"ItemId":"1","Quantity":2},{"ItemId":"2","Quantity":3}]""")
            .GetJsonAsync<HelloWorld.Response>();

        await Verify(response).DontScrubGuids();
    }
}