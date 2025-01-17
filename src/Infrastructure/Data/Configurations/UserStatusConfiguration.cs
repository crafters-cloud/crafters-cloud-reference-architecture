using CraftersCloud.Core.SmartEnums.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UserStatusConfiguration : IEntityTypeConfiguration<UserStatus>
{
    public void Configure(EntityTypeBuilder<UserStatus> builder)
    {
        builder.Property(x => x.Id).HasColumnType("tinyint");
        builder.ConfigureEntityWithEnumId<UserStatus, UserStatusId>(UserStatus.NameMaxLength);
    }
};