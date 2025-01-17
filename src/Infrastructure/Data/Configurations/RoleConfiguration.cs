using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly]
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Id).HasStronglyTypedId(x => x.Value, x => RoleId.Create(x));
        builder.Property(x => x.Name).IsRequired().HasMaxLength(Role.NameMaxLength);

        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .HasMany(role => role.Permissions)
            .WithMany(permission => permission.Roles)
            .UsingEntity<RolePermission>(
                right => right
                    .HasOne<Permission>()
                    .WithMany()
                    .HasForeignKey(x => x.PermissionId),
                left => left
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(x => x.RoleId)
            );
    }
}