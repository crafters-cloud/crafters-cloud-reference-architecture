using CraftersCloud.Core.Entities;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using GetUserById = CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld.GetUserById;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]

// Basic example of an integration test.
// For every public api method add appropriate test.
// Integration tests should be used for happy flows.
// For non-happy flows (e.g. edge cases), or complex business rules
// write unit tests.
public class UserEndpointsFixture : EndpointsFixtureBase
{
    private IFlurlRequest _endpoint = null!;

    private User _user = null!;

    [SetUp]
    public void SetUp()
    {
        _endpoint = Client.Request("users");

        _user = new UserBuilder()
            .WithEmailAddress("john_doe@john.doe")
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithRoleId(Role.SystemAdminRoleId)
            .WithStatusId(UserStatusId.Active);

        AddAndSaveChanges(_user);
    }

    [Test]
    public async Task GetAll()
    {
        var response = await _endpoint.AppendQueryParam(nameof(GetUsers.Request.SortBy), nameof(User.EmailAddress))
            .AppendQueryParam(nameof(GetUsers.Request.EmailAddress), "john_doe")
            .GetJsonAsync<PagedQueryResponse<GetUsers.Response.Item>>();
        await Verify(response);
    }

    [Test]
    public async Task GetById()
    {
        var response = await _endpoint.AppendPathSegment(_user.Id)
            .GetJsonAsync<GetUserById.Response>();
        await Verify(response);
    }

    [Test]
    public async Task GetStatuses()
    {
        var response = await _endpoint.AppendPathSegment("statuses").GetJsonAsync<GetUserStatuses.Response>();
        await Verify(response);
    }

    [Test]
    public async Task Create()
    {
        var request = new CreateUser.Request(
            "someuser@test.com",
            "some",
            "user",
            Role.SystemAdminRoleId,
            UserStatusId.Active
        );
        var response = await _endpoint.PostJsonAsync(request);
        response.StatusCode.ShouldBe((int) HttpStatusCode.Created);
    }

    [Test]
    public async Task Update()
    {
        var request = new UpdateUser.Request(
            _user.Id,
            "someuser@test.com",
            "some",
            "user",
            Role.SystemAdminRoleId,
            UserStatusId.Inactive
        );
        var response = await _endpoint.PutJsonAsync(request);
        response.StatusCode.ShouldBe((int) HttpStatusCode.NoContent);
        var user = QueryDb<User>().QueryById(_user.Id).Single();
        await Verify(user);
    }
}