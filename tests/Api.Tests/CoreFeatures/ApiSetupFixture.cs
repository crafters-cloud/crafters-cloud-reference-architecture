using System.Net;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;
using CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using GetUsers = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.GetUsers;
using UpdateUser = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.UpdateUser;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

// Fixture that validates if Api project has been setup correctly
// Uses arbitrary endpoints (in this case user related) and verifies if basic operations, e.g. get, post, validations are correctly setup.  
[Category("integration")]
public class ApiSetupFixture : IntegrationFixtureBase
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
    public async Task TestGetAll()
    {
        var users = await Client.Request("users")
            .AppendQueryParam("SortBy", "EmailAddress")
            .GetJsonAsync<PagedQueryResponse<GetUsers.Response.Item>>();
        await Verify(users);
    }

    [Test]
    public async Task GivenValidUserId_GetById_ReturnsUserDetails()
    {
        var user = await Client.Request("users").AppendPathSegment(_user.Id).GetJsonAsync<GetUserDetails.Response>();
        await Verify(user);
    }

    [Test]
    public async Task GivenNonExistingUserId_GetById_ReturnsNotFound()
    {
        var response = await Client.Request($"users").AppendPathSegment(Guid.NewGuid()).AllowHttpStatus(404).GetAsync();
        response.StatusCode.Should().Be(404);
    }

    [Test]
    public async Task CreateUser()
    {
        var request = new CreateUser.Request(
            "someuser@test.com", "some user",
            Role.SystemAdminRoleId,
            UserStatusId.Inactive);

        var result = await Client.Request("users").PostJsonAsync(request);
        result.StatusCode.Should().Be((int) HttpStatusCode.Created);
        var (_, location) = result.Headers.FirstOrDefault(h => h.Name == "Location");
        location.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task UpdateUser()
    {
        var request = new UpdateUser.Request(
            _user.Id,
            FullName: "some user",
            EmailAddress: "someuser@test.com",
            RoleId: Role.SystemAdminRoleId,
            UserStatusId: UserStatusId.Inactive
        );
        var response = await Client.Request("users").PutJsonAsync(request);
        response.StatusCode.Should().Be((int) HttpStatusCode.NoContent);
        var user = QueryByIdSkipCache<User>(_user.Id);
        await Verify(user);
    }

    [TestCase("some user", "invalid email", TestName = "InvalidEmail")]
    [TestCase("", "someuser@test.com", TestName = "MissingName")]
    [TestCase("some user", "", TestName = "MissingEmail")]
    [TestCase("John Doe", "john_doe@john.doe", TestName = "EmailAddressAlreadyTaken")]
    public async Task CreateReturnsValidationErrors(string name,
        string emailAddress)
    {
        var request = new CreateUser.Request(
            emailAddress,
            name,
            Role.SystemAdminRoleId,
            UserStatusId.Inactive
        );
        var response = await Client.Request("users").AllowHttpStatus((int) HttpStatusCode.BadRequest)
            .PostJsonAsync(request);
        var validationErrors = await response.GetJsonAsync<ValidationProblemDetails>();
        response.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        await Verify(validationErrors).UseParameters(TestContext.CurrentContext.Test.Name);
    }
}