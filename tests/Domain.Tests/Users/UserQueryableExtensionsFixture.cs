using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Domain.Tests.Users;

[Category("unit")]
public class UserQueryableExtensionsFixture
{
    private IQueryable<User> _query = null!;
    private User _user = null!;
    private User _user2 = null!;

    [SetUp]
    public void Setup()
    {
        _user = new UserBuilder()
            .WithEmailAddress("emailAddress1")
            .WithFirstName("name")
            .WithRoleId(Role.SystemAdminRoleId);
        _user2 = new UserBuilder()
            .WithEmailAddress("emailAddress2")
            .WithFirstName("name2")
            .WithRoleId(Role.SystemAdminRoleId);

        _query = new List<User> { _user, _user2 }.AsQueryable();
    }

    [Test]
    public void QueryEmptyList()
    {
        var result = new List<User>().AsQueryable().QueryByEmail("some").ToList();
        result.ShouldBeEmpty();
    }

    [TestCase("emailAddress1", 1, TestName = "Case sensitive-should find")]
    [TestCase("emailAddress2", 1, TestName = "Case sensitive-should find, v2")]
    [TestCase("EmailAddress1", 0, TestName = "Case sensitive-should not find")]
    [TestCase("EmailAddress2", 0, TestName = "Case sensitive-should not find, v2")]
    [TestCase("xyz", 0, TestName = "Should not find")]
    public void TestQueryByEmailAddress(string emailAddress, int expectedCount)
    {
        //change to use expectedCount instead of Verify
        var result = _query.QueryByEmail(emailAddress).ToList();

        result.Count.ShouldBe(expectedCount);
    }
}