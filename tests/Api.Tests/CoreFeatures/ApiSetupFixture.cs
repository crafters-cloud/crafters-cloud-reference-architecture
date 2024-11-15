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
        var users = (await Client.GetAsync<PagedResponse<GetUsers.Response.Item>>(
                new Uri("users", UriKind.RelativeOrAbsolute),
                new KeyValuePair<string, string>("SortBy", "EmailAddress")))
            ?.Items.ToList()!;

        await Verify(users);
    }

    [Test]
    public async Task GivenValidUserId_GetById_ReturnsUserDetails()
    {
        var user = await Client.GetAsync<GetUserDetails.Response>($"users/{_user.Id}");

        await Verify(user);
    }

    [Test]
    public async Task GivenNonExistingUserId_GetById_ReturnsNotFound()
    {
        var response = await Client.GetAsync($"users/{Guid.NewGuid()}");

        response.Should().BeNotFound();
    }

    [Test]
    public async Task CreateUser()
    {
        var request = new CreateUser.Request(
            "someuser@test.com", "some user",
            Role.SystemAdminRoleId,
            UserStatusId.Inactive);

        var result = await Client.PostAsJsonAsync("users", request, HttpSerializationOptions.Options);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
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
        var response = await Client.PutAsJsonAsync("users", request, HttpSerializationOptions.Options);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var user = QueryByIdSkipCache<User>(_user.Id);
        await Verify(user);
    }

    [TestCase("some user", "invalid email", "emailAddress", "is not a valid email address.")]
    [TestCase("", "someuser@test.com", "fullName", "must not be empty.")]
    [TestCase("some user", "", "emailAddress", "must not be empty.")]
    [TestCase("John Doe", "john_doe@john.doe", "emailAddress", "EmailAddress is already taken")]
    public async Task CreateReturnsValidationErrors(string name,
        string emailAddress,
        string validationField,
        string validationErrorMessage)
    {
        var request = new CreateUser.Request(
            emailAddress,
            name,
            Role.SystemAdminRoleId,
            UserStatusId.Inactive
        );
        var response = await Client.PostAsJsonAsync("users", request, HttpSerializationOptions.Options);
        response.Should().BeBadRequest().And.ContainValidationError(validationField, validationErrorMessage);
    }
}