using System.Security.Claims;
using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;

public static class TestUserData
{
    public static readonly Guid TestUserId = new("8af459b7-d6b3-4b0a-8200-66257811e66e");
    private const string TestEmailAddress = "test_user@xyz.com";

    public static ClaimsPrincipal CreateClaimsPrincipal() =>
        new(new ClaimsIdentity([new Claim(ClaimTypes.Upn, TestEmailAddress)], "TestAuth"));

    public static User CreateTestUser() =>
        CreateUser(TestUserId, TestEmailAddress, "INTEGRATION", "TEST", Role.SystemAdminRoleId);

    public static User CreateSystemUser() =>
        CreateUser(User.SystemUserId, "N/A", "System", "User", Role.SystemAdminRoleId);

    private static User CreateUser(Guid id, string email, string firstName, string lastName, Guid roleId)
    {
        var user = User.Create(new CreateUserCommand
        {
            RoleId = roleId,
            EmailAddress = email,
            FirstName = firstName,
            LastName = lastName,
            UserStatusId = UserStatusId.Active
        }).WithId(id);
        return user;
    }
}