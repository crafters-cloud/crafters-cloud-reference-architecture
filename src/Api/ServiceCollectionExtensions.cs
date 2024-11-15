using System.Reflection;
using CraftersCloud.ReferenceArchitecture.Infrastructure;

namespace CraftersCloud.ReferenceArchitecture.Api;

public static class ServiceCollectionExtensions
{
    public static void AddCoreCarter2(this IServiceCollection services, Assembly[] assembliesWithCarterModules) =>
        services.AddCarter(configurator: c =>
        {
            var carterModules = FindCarterModules(assembliesWithCarterModules);
            c.WithModules(carterModules);

            var assemblyScanner = AssemblyScanner.FindValidatorsInAssembly(AssemblyFinder.ApiAssembly);
            var validators = assemblyScanner.Select(s => s.ValidatorType).ToArray();
            c.WithValidators(validators);
            c.WithValidatorLifetime(ServiceLifetime.Singleton);
        });

    private static Type[] FindCarterModules(Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var modules = types
            .Where(t =>
                !t.IsAbstract &&
                typeof(ICarterModule).IsAssignableFrom(t) &&
                t != typeof(ICarterModule)
                && t.IsPublic
            ).ToArray();
        return modules;
    }
}