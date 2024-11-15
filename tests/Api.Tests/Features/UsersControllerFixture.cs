using System.Net;
using System.Net.Http.Json;
using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using FluentAssertions;
using GetUserDetails = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.GetUserDetails;
using GetUsers = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.GetUsers;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Features;

[Category("integration")]

// Basic example of an integration test.
// For every public api method add appropriate test.
// Integration tests should be used for happy flows.
// For un-happy flows (e.g. edge cases), or complex business rules
// write unit tests.
public class UsersControllerFixture : IntegrationFixtureBase
{
    private User _user = null!;

    [SetUp]
    public void SetUp()
    {
        _user = new UserBuilder()
            .WithEmailAddress("john_doe@john.doe")
            .WithFullName("John Doe")
            .WithRoleId(Role.SystemAdminRoleId)
            .WithStatusId(UserStatusId.Active);

        AddAndSaveChanges(_user);
    }

    [Test]
    public async Task GetAll()
    {
        var users = (await Client.GetAsync<PagedResponse<GetUsers.Response.Item>>(
                new Uri("users", UriKind.RelativeOrAbsolute),
                new KeyValuePair<string, string>("SortBy", "EmailAddress")))
            ?.Items.ToList()!;

        await Verify(users);
    }

    [Test]
    public async Task GetById()
    {
        var user = await Client.GetAsync<GetUserDetails.Response>($"users/{_user.Id}");

        await Verify(user);
    }

    [Test]
    public async Task GetRoles()
    {
        var user = await Client.GetAsync<List<GetRoles.ResponseItem>>($"users/roles");

        await Verify(user);
    }

    [Test]
    public async Task GetStatuses()
    {
        var user = await Client.GetAsync<List<GetStatuses.ResponseItem>>($"users/statuses");

        await Verify(user);
    }

    [Test]
    public async Task Create()
    {
        var request = new CreateUser.Request(
            "someuser@test.com",
            "some user",
            Role.SystemAdminRoleId,
            UserStatusId.Active
        );
        var response =
            await Client.PostAsJsonAsync("users", request, HttpSerializationOptions.Options);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Update()
    {
        var request = new UpdateUser.Request(
            _user.Id,
            "someuser@test.com",
            "some user",
            Role.SystemAdminRoleId,
            UserStatusId.Inactive
        );
        var response = await Client.PutAsJsonAsync("users", request, HttpSerializationOptions.Options);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var user = QueryByIdSkipCache<User>(_user.Id);
        await Verify(user);
    }
}