﻿using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly]
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(permission => permission.Name).IsRequired().HasMaxLength(Permission.NameMaxLength);
        builder.HasIndex(permission => permission.Name).IsUnique();
    }
}