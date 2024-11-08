using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Features;
using CraftersCloud.ReferenceArchitecture.Api.Features.Users;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

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
        var command = new CreateOrUpdateUser.Command
        {
            Id = null,
            FullName = "some user",
            EmailAddress = "someuser@test.com",
            RoleId = Role.SystemAdminRoleId,
            UserStatusId = UserStatusId.Active
        };
        var user =
            await Client.PostAsync<CreateOrUpdateUser.Command, GetUserDetails.Response>("users", command);

        await Verify(user);
    }

    [Test]
    public async Task Update()
    {
        var command = new CreateOrUpdateUser.Command
        {
            Id = _user.Id,
            FullName = "some user",
            EmailAddress = "someuser@test.com",
            RoleId = Role.SystemAdminRoleId,
            UserStatusId = UserStatusId.Inactive
        };
        var user =
            await Client.PostAsync<CreateOrUpdateUser.Command, GetUserDetails.Response>("users", command);

        await Verify(user);
    }
}