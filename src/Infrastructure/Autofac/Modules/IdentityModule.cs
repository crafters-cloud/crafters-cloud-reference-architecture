using Autofac;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;
using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Autofac.Modules;

[UsedImplicitly]
public class IdentityModule<T> : Module where T : ICurrentUserProvider
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ClaimsProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<T>().AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}