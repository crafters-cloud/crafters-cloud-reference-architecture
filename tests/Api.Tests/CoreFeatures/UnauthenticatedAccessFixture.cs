namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

[Category("integration")]
public class UnauthenticatedAccessFixture : EndpointsFixtureBase
{
    public UnauthenticatedAccessFixture() => DisableUserAuthentication();

    [Test]
    public async Task EndpointWithAuthorizeAttributeIsNotAllowed()
    {
        var response = await Client.Request("users").AllowHttpStatus((int) HttpStatusCode.Unauthorized).GetAsync();
        response.StatusCode.ShouldBe((int) HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task EndpointWithNoAuthorizeAttributeIsNotAllowed()
    {
        var response = await Client.Request("profile").AllowHttpStatus((int) HttpStatusCode.Unauthorized).GetAsync();
        response.StatusCode.ShouldBe((int) HttpStatusCode.Unauthorized);
    }
}