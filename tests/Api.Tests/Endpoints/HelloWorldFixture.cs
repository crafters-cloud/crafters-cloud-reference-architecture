using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using GetUserById = CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld.GetUserById;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]
public class HelloWorldFixture : EndpointsFixtureBase
{
    private User _user = null!;
    private IFlurlRequest _endpoint;

    [SetUp]
    public void SetUp()
    {
        _endpoint = Client.Request("hello-world");

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
        var response = await _endpoint
            .AppendPathSegment("user")
            .AppendPathSegment(_user.Id)
            .GetJsonAsync<GetUserById.Response>();

        await Verify(response);
    }
}