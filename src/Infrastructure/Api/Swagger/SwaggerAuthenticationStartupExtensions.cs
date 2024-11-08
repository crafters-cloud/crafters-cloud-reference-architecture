using CraftersCloud.Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.AspNetCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Swagger;

public static class SwaggerAuthenticationStartupExtensions
{
    public static void AppUseSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetRequiredSection(SwaggerSettings.SectionName).Get<SwaggerSettings>()!;
        if (swaggerSettings.Enabled)
        {
            app.UseCoreSwagger("/api");
        }
    }

    public static void AppAddSwagger(this IServiceCollection services,
        string appTitle,
        string appVersion = "v1",
        Action<AspNetCoreOpenApiDocumentGeneratorSettings>? configureSettings = null) =>
        services.AddCoreSwagger(appTitle, appVersion, configureSettings);
}