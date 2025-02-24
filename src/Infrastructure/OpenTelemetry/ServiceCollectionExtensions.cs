using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    public static void AppAddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        {
            services.AddOpenTelemetry()
                .UseAzureMonitor();
            
            // how to configure instrumentation options
            // https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-dotnet-migrate?tabs=aspnetcore#customizing-aspnetcoretraceinstrumentationoptions
            // services.Configure<AspNetCoreTraceInstrumentationOptions>(options =>
            // {
            //     options.RecordException = true;
            //     options.Filter = (httpContext) =>
            //     {
            //         // only collect telemetry about HTTP GET requests
            //         return HttpMethods.IsGet(httpContext.Request.Method);
            //     };
            // });
        }
    }
}