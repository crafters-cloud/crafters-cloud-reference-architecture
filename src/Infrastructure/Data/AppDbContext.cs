﻿using CraftersCloud.Core.EntityFramework.Infrastructure;
using CraftersCloud.Core.SmartEnums.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

[UsedImplicitly]
public class AppDbContext(DbContextOptions options) : BaseDbContext(CreateOptions(), options)
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<string>()
            .HaveMaxLength(255);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // first we need to build the model so that we can later configure the smart enums
        base.OnModelCreating(modelBuilder);
        modelBuilder.CoreConfigureSmartEnums();
    }

    private static EntityRegistrationOptions CreateOptions() => new()
    {
        ConfigurationAssembly = AssemblyFinder.InfrastructureAssembly, 
        EntitiesAssembly = AssemblyFinder.DomainAssembly,
        EntityTypePredicate = null,
        ConfigurationTypePredicate = null
    };
}