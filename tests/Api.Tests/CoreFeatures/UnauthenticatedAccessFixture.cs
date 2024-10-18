using System.Net;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using FluentAssertions;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

[Category("integration")]
public class UnauthenticatedAccessFixture : IntegrationFixtureBase
{
    public UnauthenticatedAccessFixture() => DisableUserAuthentication();

    [Test]
    public async Task EndpointWithAuthorizeAttributeIsNotAllowed()
    {
        var response = await Client.GetAsync("api/users");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task EndpointWithNoAuthorizeAttributeIsNotAllowed()
    {
        var response = await Client.GetAsync("api/profile");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}