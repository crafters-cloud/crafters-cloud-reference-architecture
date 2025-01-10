using CraftersCloud.ReferenceArchitecture.Domain.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

public static class EntityTypeBuilderExtensions
{
    public static void HasCreatedByAndUpdatedBy<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithCreatedUpdated
    {
        builder.HasOne<User>().WithMany().HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne<User>().WithMany().HasForeignKey(e => e.UpdatedById).OnDelete(DeleteBehavior.NoAction);
    }
}