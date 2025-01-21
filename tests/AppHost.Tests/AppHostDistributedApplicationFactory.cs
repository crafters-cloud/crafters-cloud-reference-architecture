using CraftersCloud.ReferenceArchitecture.AppHost.Tests.Infrastructure;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.ReferenceArchitecture.AppHost.Tests;

internal static class DistributedApplicationTestFactory
{
    public static async Task<IDistributedApplicationTestingBuilder> CreateAsync<TEntryPoint>() where TEntryPoint : class
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<TEntryPoint>();

        builder.WithRandomParameterValues();
        builder.WithRandomVolumeNames();
        builder.WithContainersLifetime(ContainerLifetime.Session);

        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSimpleConsole();
            logging.AddFakeLogging();
            logging.AddNUnit();
            logging.SetMinimumLevel(LogLevel.Trace);
            logging.AddFilter("Aspire", LogLevel.Trace);
            logging.AddFilter(builder.Environment.ApplicationName, LogLevel.Trace);
        });

        return builder;
    }
}