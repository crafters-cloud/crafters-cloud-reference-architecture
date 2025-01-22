using CraftersCloud.ReferenceArchitecture.Data.Migrations;
using CraftersCloud.ReferenceArchitecture.Data.MigrationService;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;
using CraftersCloud.ReferenceArchitecture.Migrations;
using CraftersCloud.ReferenceArchitecture.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<AppDbContext>(nameof(AppDbContext), null,
    optionsBuilder =>
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        optionsBuilder.UseSqlServer(sqlServerOptions =>
            sqlServerOptions.MigrationsAssembly(typeof(AppDesignTimeDbContextFactory).Assembly.FullName));
    });

var host = builder.Build();
await host.RunAsync();