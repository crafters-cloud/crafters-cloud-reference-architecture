using Autofac;
using CraftersCloud.Core;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Autofac;

[UsedImplicitly]
public class TestModule : Module
{
    protected override void Load(ContainerBuilder builder) =>
        builder.RegisterType<SettableTimeProvider>()
            .As<ITimeProvider>()
            .AsSelf()
            .SingleInstance();
}