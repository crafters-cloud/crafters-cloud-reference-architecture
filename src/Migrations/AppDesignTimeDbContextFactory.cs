﻿using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;
using CraftersCloud.ReferenceArchitecture.Migrations.Seeding.MigrationSeeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CraftersCloud.ReferenceArchitecture.Migrations;

[UsedImplicitly]
public class AppDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args) => CreateDbContext(ReadConnectionString(args));

    private static AppDbContext CreateDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString,
            b =>
            {
                b.MigrationsAssembly(typeof(AppDesignTimeDbContextFactory).Assembly.FullName);
            });

        var result = new AppDbContext(optionsBuilder.Options)
        {
            ModelBuilderConfigurator = MigrationSeeding.Seed
        };
        return result;
    }

    private static string ReadConnectionString(string[] args)
    {
        var connectionString = args.FirstOrDefault();

        if (!string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        Console.WriteLine("Connection string is not provided in the arguments.'");
        return string.Empty;
    }
}