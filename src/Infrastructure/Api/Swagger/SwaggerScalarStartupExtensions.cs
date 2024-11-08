using CraftersCloud.Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.AspNetCore;
using Scalar.AspNetCore;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Swagger;

public static class SwaggerScalarStartupExtensions
{
    public static void AppAddSwaggerScalar(this IServiceCollection services,
        string appTitle,
        string appVersion,
        IConfiguration configuration,
        Action<AspNetCoreOpenApiDocumentGeneratorSettings>? configureSettings = null)
    {
        if (!SwaggerEnabled(configuration))
        {
            return;
        }

        services.AddEndpointsApiExplorer();
        services.AddCoreSwagger(appTitle, appVersion, configureSettings);
    }

    public static void AppUseSwaggerScalar(this WebApplication app, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetRequiredSection(SwaggerSettings.SectionName).Get<SwaggerSettings>()!;
        if (!swaggerSettings.Enabled)
        {
            return;
        }

        app.UseOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.Path = "/swagger";
        });
        app.MapScalarApiReference(options =>
        {
            options
                .WithOpenApiRoutePattern("/swagger/v1/swagger.json")
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    private static bool SwaggerEnabled(IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetRequiredSection(SwaggerSettings.SectionName).Get<SwaggerSettings>()!;
        return swaggerSettings.Enabled;
    }
}