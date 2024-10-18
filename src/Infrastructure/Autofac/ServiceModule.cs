using System.Reflection;
using Autofac;
using Module = Autofac.Module;
using TimeProvider = CraftersCloud.Core.Infrastructure.TimeProvider;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Autofac;

public class ServiceModule : Module
{
    public IEnumerable<Assembly> Assemblies { get; set; } = [];

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(Assemblies.ToArray())
            .Where(type => type.Name.EndsWith("Service", StringComparison.InvariantCulture))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.RegisterType<TimeProvider>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}