using Autofac;
using Autofac.Extensions.DependencyInjection;
using CraftersCloud.Core.AspNetCore.Authorization;
using CraftersCloud.Core.AspNetCore.Exceptions;
using CraftersCloud.Core.AspNetCore.Security;
using CraftersCloud.Core.HealthChecks.Extensions;
using CraftersCloud.Core.SmartEnums.Swagger;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Logging;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Startup;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Swagger;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Autofac;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Configuration;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Mediator;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace CraftersCloud.ReferenceArchitecture.Api;

public static class ProgramExtensions
{
    public static void AppAddServices(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services.AddCors();
        services.AddHttpContextAccessor();
        services.AddApplicationInsightsTelemetry();

        services.AppConfigureSettings(configuration);
        services.AppAddPolly();
        services.AppAddAutoMapper();
        services.AddCoreHealthChecks(configuration)
            .AddDbContextCheck<AppDbContext>();
        services.AppAddMediatr(AssemblyFinder.ApiAssembly);
        services.AppAddFluentValidation();
        services.AddCoreHttps(env);

        services.AppAddAuthentication();
        services.AddCoreAuthorization<PermissionId>();

        services.AppAddSwagger("Crafters Cloud Reference Architecture Api", "v1", configureSettings =>
        {
            configureSettings.CoreConfigureSmartEnums();
        });
        services.AppAddMvc();
    }

    public static void AppConfigureHost(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .AppConfigureSerilog(configuration)
                .AddAppInsightsToSerilog(configuration, services);
        });
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        hostBuilder.ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
        {
            containerBuilder.AppRegisterModules();
            containerBuilder.AppRegisterClaimsPrincipalProvider();
            containerBuilder.RegisterModule<IdentityModule<CurrentUserProvider>>();
        });
    }

    public static void AppConfigureWebApplication(this WebApplication app)
    {
        var configuration = app.Configuration;

        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                if (context.File.Name != "index.html")
                {
                    context.Context.Response.Headers.Append("Cache-Control", "public, max-age: 604800");
                }
            }
        });

        app.MapFallbackToFile("index.html");

        app.UseRouting();

        if (configuration.AppUseDeveloperExceptionPage())
        {
            app.UseDeveloperExceptionPage();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseCors(builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
            IdentityModelEventSource.ShowPII = true;
        }

        app.UseCoreHttps(app.Environment);
        app.UseCoreExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<LogContextMiddleware>();

        app.MapControllers().RequireAuthorization();
        app.MapCoreHealthChecks(configuration);

        app.AppUseSwagger(configuration);

        app.AppConfigureFluentValidation();
    }
}