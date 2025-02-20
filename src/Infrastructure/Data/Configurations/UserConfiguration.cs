﻿using CraftersCloud.Core.SmartEnums.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly]
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).HasStronglyTypedId(x => x.Value, x => UserId.Create(x));
        builder.Property(u => u.EmailAddress).IsRequired().HasMaxLength(User.EmailAddressMaxLength);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(User.FirstNameMaxLength);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(User.LastNameMaxLength);
        builder.Property(u => u.RoleId).IsRequired();
        builder.Property(u => u.CreatedOn).IsRequired();

        builder.HasIndex(u => u.EmailAddress).IsUnique();

        builder.HasReferenceTableRelationWithEnumAsForeignKey(x => x.UserStatus, x => x.UserStatusId);

        builder.HasOne(u => u.Role).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.HasCreatedByAndUpdatedBy();
    }
}