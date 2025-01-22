using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Migrations.Seeding.MigrationSeeding;

public class RolePermissionSeeding : IModelBuilderSeeding
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>().HasData(AllPermissions());

        modelBuilder.Entity<Role>().HasData(AllRoles());

        var rolePermissions = GetRolePermissions();
        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }

    private static IEnumerable<object> AllRoles()
    {
        yield return new { Id = Role.SystemAdminRoleId, Name = "SystemAdmin" };
    }

    private static IEnumerable<Permission> AllPermissions() =>
        Enum.GetValues<PermissionId>()
            .Select(permissionId => new Permission(permissionId));

    private static List<RolePermission> GetRolePermissions() =>
        GetSystemUserPermissions()
            .SelectMany(tuple => tuple.Permissions
                .Select(permissionId => new RolePermission { RoleId = tuple.RoleId, PermissionId = permissionId }))
            .ToList();

    private static IEnumerable<(RoleId RoleId, PermissionId[] Permissions)> GetSystemUserPermissions()
    {
        yield return new ValueTuple<RoleId, PermissionId[]>(Role.SystemAdminRoleId,
            [.. Enum.GetValues<PermissionId>()]); // all permissions
    }
}