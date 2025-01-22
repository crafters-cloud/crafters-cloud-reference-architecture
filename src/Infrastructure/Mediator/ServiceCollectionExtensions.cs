using System.Reflection;
using CraftersCloud.Core.MediatR;
using CraftersCloud.Core.MediatR.Caching;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Mediator;

public static class ServiceCollectionExtensions
{
    public static void AppAddMediatr(this IServiceCollection services, Assembly entryAssembly)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AppSaveChangesBehavior<,>));

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(
                entryAssembly,
                AssemblyFinder.DomainAssembly,
                AssemblyFinder.ApplicationAssembly);
            
        });
        
        // register cache invalidation
        services.AddScoped(typeof(INotificationHandler<>), typeof(CacheEvictorNotificationHandler<>));
    }
}