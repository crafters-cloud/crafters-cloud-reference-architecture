using CraftersCloud.Core.EntityFramework.Seeding;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Data.Migrations.Seeding;

public class RolePermissionSeeding : ISeeding
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>().HasData(CreateAllPermissions());

        modelBuilder.Entity<Role>().HasData(CreateAllRoles());

        var rolePermissions = GetRolePermissions();
        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }

    private static IEnumerable<Role> CreateAllRoles()
    {
        yield return new Role { Id = Role.SystemAdminRoleId, Name = "SystemAdmin" };
    }
    
    private static IEnumerable<Permission> CreateAllPermissions() =>
        Enum.GetValues<PermissionId>()
            .Select(permissionId => new Permission(permissionId));

    private static List<RolePermission> GetRolePermissions() =>
        GetSystemUserPermissions()
            .SelectMany(tuple => tuple.Permissions
                .Select(permissionId => new RolePermission { RoleId = tuple.RoleId, PermissionId = permissionId }))
            .ToList();
    
    private static IEnumerable<(Guid RoleId, PermissionId[] Permissions)> GetSystemUserPermissions()
    {
        yield return new ValueTuple<Guid, PermissionId[]>(Role.SystemAdminRoleId,
            [.. Enum.GetValues<PermissionId>()]); // all permissions
    }
}