using System.Reflection;
using CraftersCloud.Core.MediatR;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Mediator;

public static class ServiceCollectionExtensions
{
    public static void AppAddMediatr(this IServiceCollection services, params Assembly[] extraAssemblies)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AppSaveChangesBehavior<,>));

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(extraAssemblies);
            config.RegisterServicesFromAssemblies(
                AssemblyFinder.DomainAssembly,
                AssemblyFinder.ApplicationServicesAssembly);
        });
    }
}