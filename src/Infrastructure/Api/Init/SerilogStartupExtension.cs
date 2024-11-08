using System.Reflection;
using CraftersCloud.Core.AspNetCore.ApplicationInsights;
using CraftersCloud.Core.Configuration;
using CraftersCloud.Core.Helpers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class SerilogStartupExtension
{
    public static LoggerConfiguration AppConfigureSerilog(this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration)
    {
        var loggerSectionExists = configuration.GetSection("Serilog").Exists();
        if (!loggerSectionExists)
        {
            // we might not have logger section in the tests only
            return loggerConfiguration;
        }

        loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithMachineName()
            .Enrich.With(new OperationIdEnricher())
            .Enrich.WithProperty("AppVersion", Assembly.GetEntryAssembly()!.GetName().Version!);

        // for enabling self diagnostics see https://github.com/serilog/serilog/wiki/Debugging-and-Diagnostics
        // Serilog.Debugging.SelfLog.Enable(Console.Error);

        return loggerConfiguration;
    }

    public static void AddAppInsightsToSerilog(this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var settings = configuration.GetRequiredSection(ApplicationInsightsSettings.SectionName)
            .Get<ApplicationInsightsSettings>()!;
        if (!settings.ConnectionString.HasContent())
        {
            return;
        }

        var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
        loggerConfiguration.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces,
            LogEventLevel.Information);
    }
}