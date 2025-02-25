using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using CraftersCloud.Core.Helpers;
using Serilog;

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
        
        var assembly = Assembly.GetEntryAssembly()!;
        var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var productVersion = versionInfo.ProductVersion.RemoveAfter('+');

        loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("ProductVersion", productVersion)
            .WriteTo.OpenTelemetry();

        // for enabling self diagnostics see https://github.com/serilog/serilog/wiki/Debugging-and-Diagnostics
        // Serilog.Debugging.SelfLog.Enable(Console.Error);

        return loggerConfiguration;
    }
}