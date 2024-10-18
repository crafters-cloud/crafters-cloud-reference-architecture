using System.Reflection;
using CraftersCloud.Core.MediatR;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Init;

public static class MediatRStartupExtensions
{
    public static void AppAddMediatR(this IServiceCollection services, params Assembly[] extraAssemblies)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(extraAssemblies);
            config.RegisterServicesFromAssemblies(
                AssemblyFinder.DomainAssembly,
                AssemblyFinder.ApplicationServicesAssembly);
        });
    }
}