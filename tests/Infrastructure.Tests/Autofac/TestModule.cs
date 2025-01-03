using Autofac;
using CraftersCloud.Core;
using CraftersCloud.Core.TestUtilities;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Autofac;

[UsedImplicitly]
public class TestModule : Module
{
    protected override void Load(ContainerBuilder builder) =>
        builder.RegisterType<TestTimeProvider>()
            .As<ITimeProvider>()
            .As<TestTimeProvider>()
            .SingleInstance();
}