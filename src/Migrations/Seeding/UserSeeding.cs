using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding;

internal class UserSeeding : ISeeding
{
    public void Seed(ModelBuilder modelBuilder)
    {
        var asOf = new DateTimeOffset(2024, 3, 29, 0, 0, 0, TimeSpan.Zero);
        var user = new
        {
            Id = User.SystemUserId,
            EmailAddress = "N/A",
            FullName = "System User",
            RoleId = Role.SystemAdminRoleId,
            CreatedById = User.SystemUserId,
            CreatedOn = asOf,
            UpdatedById = User.SystemUserId,
            UpdatedOn = asOf,
            UserStatusId = UserStatusId.Active
        };
        modelBuilder.Entity<User>().HasData(user);
    }
}