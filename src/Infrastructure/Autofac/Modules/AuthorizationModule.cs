using Autofac;
using CraftersCloud.Core.AspNetCore.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;
using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Autofac.Modules;

[UsedImplicitly]
public class AuthorizationModule : Module
{
    protected override void Load(ContainerBuilder builder) => builder.RegisterType<DefaultAuthorizationProvider>()
        .As<IAuthorizationProvider<PermissionId>>().InstancePerLifetimeScope();
}