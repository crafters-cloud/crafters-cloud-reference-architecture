using CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.Extensions.Caching.Hybrid;
using GetUserById = CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld.GetUserById;
using UpdateUser = CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users.UpdateUser;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.CoreFeatures;

// Fixture that validates if caching has been setup correctly
// Uses arbitrary endpoints (in this case user related) and verifies if basic operations, populates and invalidates the cache 
[Category("integration")]
public class CacheSetupFixture : EndpointsFixtureBase
{
    private HybridCache _cache;
    private IFlurlRequest Endpoint => Client.Request("users");

    [SetUp]
    public void SetUp() => _cache = Resolve<HybridCache>();

    [Test]
    public async Task GetUserPopulatesCache_UpdateDeletesCache()
    {
        var userId = await CreateUser();
        var cacheKey = "Application:Identity:GetUserById:Query:" + userId;

        var existsInCache = await _cache.ExistsAsync(cacheKey);
        existsInCache.ShouldBeFalse("Cache is empty");

        // get user by id to populate cache
        var response = await GetUserBy(userId);
        response.Id.ShouldBe(userId);

        existsInCache = await _cache.ExistsAsync(cacheKey);
        existsInCache.ShouldBeTrue("Getting user by id populates cache");

        // now call update, which clears cache
        var statusCode = await UpdateUser(userId);
        statusCode.ShouldBe(HttpStatusCode.NoContent);
        existsInCache = await _cache.ExistsAsync(cacheKey);
        existsInCache.ShouldBeFalse("Update clears the cache");
    }

    private async Task<UserId> CreateUser()
    {
        var request = ACreateUserRequest();
        var result = await Endpoint.PostJsonAsync(request);
        result.StatusCode.ShouldBe((int) HttpStatusCode.Created);
        var (_, location) = result.Headers.FirstOrDefault(h => h.Name == "Location");
        var userIdsStr = location.Replace("/users/", string.Empty);
        var userId = Guid.Parse(userIdsStr);
        return UserId.Create(userId);
    }

    private async Task<HttpStatusCode> UpdateUser(UserId id)
    {
        var request = AnUpdateUserRequest(id);
        var result = await Endpoint.PutJsonAsync(request);
        return (HttpStatusCode) result.StatusCode;
    }

    private async Task<GetUserById.Response> GetUserBy(UserId id)
    {
        var response = await Endpoint.AppendPathSegment(id).GetJsonAsync<GetUserById.Response>();
        response.Id.ShouldBe(id);
        return response;
    }

    private static CreateUser.Request ACreateUserRequest() =>
        new(
            "someuser2@test.com",
            "some",
            "user",
            Role.SystemAdminRoleId,
            UserStatusId.Active);

    private static UpdateUser.Request AnUpdateUserRequest(UserId userId) =>
        new(
            userId,
            "someuser2@test.com",
            "some1",
            "user2",
            Role.SystemAdminRoleId,
            UserStatusId.Active);
}