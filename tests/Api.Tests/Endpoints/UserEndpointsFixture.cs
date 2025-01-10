using System.Net;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using FluentAssertions;
using Flurl.Http;
using GetUsers = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.GetUsers;
using UpdateUser = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.UpdateUser;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Endpoints;

[Category("integration")]

// Basic example of an integration test.
// For every public api method add appropriate test.
// Integration tests should be used for happy flows.
// For non-happy flows (e.g. edge cases), or complex business rules
// write unit tests.
public class UserEndpointsFixture : IntegrationFixtureBase
{
    private User _user = null!;

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
    public async Task GetAll()
    {
        var response = await Client.Request("users").AppendQueryParam("SortBy", "EmailAddress")
            .GetJsonAsync<PagedQueryResponse<GetUsers.Response.Item>>();
        await Verify(response);
    }

    [Test]
    public async Task GetById()
    {
        var response = await Client.Request("users").AppendPathSegment(_user.Id)
            .GetJsonAsync<GetUserById.Response>();
        await Verify(response);
    }

    [Test]
    public async Task GetStatuses()
    {
        var response = await Client.Request("users").AppendPathSegment("statuses").GetJsonAsync<GetStatuses.Response>();
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
        var response =
            await Client.Request("users").PostJsonAsync(request);
        response.StatusCode.Should().Be((int) HttpStatusCode.Created);
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
        var response = await Client.Request("users").PutJsonAsync(request);
        response.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        var user = QueryByIdSkipCache<User>(_user.Id);
        await Verify(user);
    }
}