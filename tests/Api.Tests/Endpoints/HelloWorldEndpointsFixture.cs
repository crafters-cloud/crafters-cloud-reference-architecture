using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using GetUserById = CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld.GetUserById;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class HelloWorldEndpointsFixture : EndpointsFixtureBase
{
    private User _user = null!;
    private IFlurlRequest Endpoint => Client.Request("hello-world");

    [SetUp]
    public void SetUp()
    {
        _user = new UserBuilder()
            .WithEmailAddress("john_doe@john.doe")
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithRoleId(Role.SystemAdminRoleId)
            .WithStatusId(UserStatusId.Active);

        AddAndSaveChanges(_user);
    }

    [Test]
    public async Task HelloWorld()
    {
        var response = await Endpoint
            .AppendPathSegment("user")
            .AppendPathSegment(_user.Id)
            .GetJsonAsync<GetUserById.Response>();

        await Verify(response);
    }
}