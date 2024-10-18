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

        modelBuilder.Entity<Role>().HasData(Role.CreateAll());

        var rolePermissions = GetRolePermissions();
        modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
    }

    private static List<RolePermission> GetRolePermissions() =>
        Role.GetAllRolePermissions()
            .SelectMany(tuple => tuple.Permissions
                .Select(permissionId => new RolePermission { RoleId = tuple.RoleId, PermissionId = permissionId }))
            .ToList();

    private static IEnumerable<Permission> CreateAllPermissions() =>
        Enum.GetValues<PermissionId>()
            .Except([PermissionId.None])
            .Select(permissionId => new Permission(permissionId));
}