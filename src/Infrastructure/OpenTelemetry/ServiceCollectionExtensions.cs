using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Trace;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    public static void AppAddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var azureMonitorConnectionString = configuration["AzureMonitor:ConnectionString"];
        if (string.IsNullOrEmpty(azureMonitorConnectionString))
        {
            return;
        }

        // how to configure instrumentation options
        // https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-dotnet-migrate?tabs=aspnetcore#customizing-aspnetcoretraceinstrumentationoptions
        services.AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = azureMonitorConnectionString;
                options.SamplingRatio =
                    1.0F; // In production set the sampling ratio to less to avoid high costs (e.g. 10%). This wold mean that 10% of all traces will be sampled and sent to Azure Monitor.
            });

        //AspNetCore
        services.Configure<AspNetCoreTraceInstrumentationOptions>(options =>
        {
            options.RecordException = true;
            // define which HttpMethods to capture
            options.Filter = httpContext =>
            {
                var method = httpContext.Request.Method;
                return HttpMethods.IsGet(method)
                       || HttpMethods.IsPost(method)
                       || HttpMethods.IsPut(method)
                       || HttpMethods.IsDelete(method)
                       || HttpMethods.IsPatch(method);
            };
        });

        //HttpClients
        services.Configure<HttpClientTraceInstrumentationOptions>(options =>
        {
            options.RecordException = true;
            options.FilterHttpRequestMessage = httpRequestMessage =>
            {
                var method = httpRequestMessage.Method.Method;
                return HttpMethods.IsGet(method);
            };
        });
    }
}