using Autofac;
using CraftersCloud.Core.Configuration;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.EntityFramework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Module = Autofac.Module;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data;

[UsedImplicitly]
public class EntityFrameworkModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.CoreRegisterRepositoryTypes([AssemblyFinder.InfrastructureAssembly]);

        // Registering interceptors as self because we want to resolve them individually to add them to the DbContextOptions in the correct order
        builder.RegisterType<PopulateCreatedUpdatedInterceptor>().AsSelf().InstancePerLifetimeScope();
        builder.RegisterType<PublishDomainEventsInterceptor>().AsSelf().InstancePerLifetimeScope();

        builder.Register(CreateDbContextOptions).As<DbContextOptions>().InstancePerLifetimeScope();

        // Needs to be registered both as self and as DbContext or the tests might not work as expected
        builder.RegisterType<AppDbContext>().AsSelf().As<DbContext>().InstancePerLifetimeScope();
        builder.RegisterType<DbContextUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
    }

    private DbContextOptions CreateDbContextOptions(IComponentContext container)
    {
        var loggerFactory = container.Resolve<ILoggerFactory>();
        var configuration = container.Resolve<IConfiguration>();

        var dbContextSettings = container.Resolve<IOptions<DbContextSettings>>().Value;

        var optionsBuilder = new DbContextOptionsBuilder();

        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging(dbContextSettings.SensitiveDataLoggingEnabled)
            //ef 9 started showing this error in integration tests
            .ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("AppDbContext")!,
            sqlOptions => SetupSqlOptions(sqlOptions, dbContextSettings));

        optionsBuilder.AddInterceptors();

        // Interceptors will be executed in the order they are added
        optionsBuilder.AddInterceptors(
            container.Resolve<PopulateCreatedUpdatedInterceptor>());
        optionsBuilder.AddInterceptors(
            container.Resolve<PublishDomainEventsInterceptor>());

        return optionsBuilder.Options;
    }

    private SqlServerDbContextOptionsBuilder SetupSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions,
        DbContextSettings dbContextSettings)
    {
        // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
        sqlOptions = sqlOptions.EnableRetryOnFailure(
            dbContextSettings.ConnectionResiliencyMaxRetryCount,
            dbContextSettings.ConnectionResiliencyMaxRetryDelay,
            null);

        if (dbContextSettings.RegisterMigrationsAssembly)
        {
            sqlOptions = sqlOptions.MigrationsAssembly("CraftersCloud.ReferenceArchitecture.Data.Migrations");
        }

        return sqlOptions;
    }
}