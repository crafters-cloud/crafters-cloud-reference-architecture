using CraftersCloud.Core.SmartEnums.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Migrations.Seeding.MigrationSeeding;

internal class UserStatusSeeding() : EntityWithEnumIdSeeding<UserStatus, UserStatusId>(statusId =>
    new { Id = statusId, statusId.Name, Description = GetDescription(statusId) })
{
    private static string GetDescription(UserStatusId statusId) =>
        statusId.Value == UserStatusId.Active
            ? "Active Status Description"
            : statusId.Value == UserStatusId.Inactive
                ? "Inactive Status Description"
                : string.Empty;
}