using Autofac;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;

[UsedImplicitly]
public class IdentityModule<T> : Module where T : ICurrentUserProvider
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ClaimsProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<T>().AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}