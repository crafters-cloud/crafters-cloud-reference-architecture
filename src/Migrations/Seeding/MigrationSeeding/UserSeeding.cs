using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Migrations.Seeding.MigrationSeeding;

internal class UserSeeding : IModelBuilderSeeding
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var asOf = new DateTimeOffset(2024, 3, 29, 0, 0, 0, TimeSpan.Zero);
        var user = new
        {
            Id = UserId.SystemUserId,
            EmailAddress = "N/A",
            FirstName = "System",
            LastName = "User",
            RoleId = Role.SystemAdminRoleId,
            CreatedById = UserId.SystemUserId,
            CreatedOn = asOf,
            UpdatedById = UserId.SystemUserId,
            UpdatedOn = asOf,
            UserStatusId = UserStatusId.Active
        };
        modelBuilder.Entity<User>().HasData(user);
    }
}