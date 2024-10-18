using Autofac;
using CraftersCloud.Core.AspNetCore.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;
using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;

[UsedImplicitly]
public class AuthorizationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ClaimsProvider>()
            .As<IClaimsProvider>().InstancePerLifetimeScope();
        builder.RegisterType<DefaultAuthorizationProvider>()
            .As<IAuthorizationProvider<PermissionId>>().InstancePerLifetimeScope();
    }
}