using System.Net;
using System.Net.Http.Json;
using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;
using FluentAssertions;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

[Category("integration")]
public class AuthorizationFixture : IntegrationFixtureBase
{
    private Role? _testRole;

    [SetUp]
    public void SetUp()
    {
        _testRole = new Role { Name = "TestRole" };
        _testRole.SetPermissions(QueryDb<Permission>().Where(p => p.Id == PermissionId.UsersRead));

        var currentUser = QueryCurrentUser();
        currentUser.UpdateRole(_testRole);
        AddAndSaveChanges(_testRole);
    }

    [Test]
    public async Task UserWithPermissionIsAllowed()
    {
        var response = await Client.GetAsync("api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task UserWithoutPermissionIsNotAllowed()
    {
        var command = new CreateOrUpdateUser.Command
        {
            Id = null,
            FullName = "some user",
            EmailAddress = "someuser@test.com",
            RoleId = Role.SystemAdminRoleId,
            UserStatusId = UserStatusId.Active
        };
        var response = await Client.PostAsJsonAsync("api/users", command, HttpSerializationOptions.Options);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task EndpointWithoutAuthorizeAttributeIsAllowed()
    {
        var response = await Client.GetAsync("api/profile");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TearDown]
    public async Task TearDown()
    {
        var currentUser = QueryCurrentUser();
        var role = QueryDb<Role>().First(r => r.Id == Role.SystemAdminRoleId);
        currentUser.UpdateRole(role);

        if (_testRole != null)
        {
            await DeleteByIdsAndSaveChangesAsync<Role, Guid>(_testRole.Id);
        }
    }

    private User QueryCurrentUser() => QueryDb<User>().First(u => u.Id == TestUserData.TestUserId);
}