using System.Net;
using System.Net.Http.Json;
using CraftersCloud.Core.AspNetCore.TestUtilities.Http;
using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;
using FluentAssertions;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using CreateUser = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.CreateUser;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

[Category("integration")]
public class AuthorizationFixture : IntegrationFixtureBase
{
    private Role? _roleWithUsersReadPermission;
    private Role? _roleWithNonePermission;

    [SetUp]
    public void SetUp()
    {
        _roleWithUsersReadPermission = CrateRole("RoleWithUsersReadPermission", PermissionId.UsersRead);
        _roleWithNonePermission = CrateRole("RoleWithNonePermission", PermissionId.None);
    }

    [Test]
    public async Task UserWithPermissionIsAllowed()
    {
        var response = await Client.Request("users").GetAsync();
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task GivenReadPermission_CurrentUserCannot_CreateUser()
    {
        await UpdateCurrentUserToRole(_roleWithUsersReadPermission!);
        var request =
            new CreateUser.Request("someuser@test.com", "some user", Role.SystemAdminRoleId, UserStatusId.Active);
        var response = await Client.Request("users").AllowHttpStatus((int)HttpStatusCode.Forbidden).PostJsonAsync(request);
        response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task GivenNonePermission_CurrentUserCannot_GetUsers()
    {
        await UpdateCurrentUserToRole(_roleWithNonePermission!);

        var response = await Client.Request("users").AllowHttpStatus((int)HttpStatusCode.Forbidden).GetAsync();
        response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task EndpointWithoutAuthorizeAttributeIsAllowed()
    {
        var response = await Client.Request("profile").GetAsync();
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [TearDown]
    public async Task TearDown()
    {
        // reset the user to system admin (to avoid permission issues in tests)
        var role = await QueryDb<Role>().QueryById(Role.SystemAdminRoleId).FirstAsync();
        await UpdateCurrentUserToRole(role);

        var dbContext = Resolve<DbContext>();
        // delete created roles (they are not reset between tests)
        if (_roleWithUsersReadPermission != null)
        {
            dbContext.Remove(_roleWithUsersReadPermission);
        }

        if (_roleWithNonePermission != null)
        {
            dbContext.Remove(_roleWithNonePermission);
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task UpdateCurrentUserToRole(Role role)
    {
        var currentUser = QueryCurrentUser();
        currentUser.UpdateRole(role);
        await SaveChangesAsync();
    }

    private Role CrateRole(string name, PermissionId permissionId)
    {
        var role = QueryDb<Role>().QueryByName(name).SingleOrDefault();
        if (role != null)
        {
            return role;
        }

        role = new Role { Name = name };
        var permission = QueryDb<Permission>().Where(p => p.Id == permissionId);
        role.SetPermissions(permission);
        AddAndSaveChanges(role);
        return role;
    }

    private User QueryCurrentUser() => QueryDb<User>().First(u => u.Id == TestUserData.TestUserId);
}