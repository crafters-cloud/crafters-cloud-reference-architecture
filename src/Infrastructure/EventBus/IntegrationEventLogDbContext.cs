using CraftersCloud.Core.IntegrationEvents.IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.EventBus;

public class IntegrationEventLogDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) => builder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);

    private static void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
    {
        builder.ToTable("IntegrationEventLog");

        builder.HasKey(e => e.EventId);

        builder.Property(e => e.EventId)
            .IsRequired();

        builder.Property(e => e.Content)
            .HasColumnType("nvarchar(MAX)")
            .IsRequired();

        builder.Property(e => e.CreationTime)
            .IsRequired();

        builder.Property(e => e.State)
            .IsRequired();

        builder.Property(e => e.TimesSent)
            .IsRequired();

        builder.Property(e => e.EventTypeName)
            .HasMaxLength(2000)
            .IsRequired();
    }
}