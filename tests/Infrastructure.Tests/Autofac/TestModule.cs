using Autofac;
using CraftersCloud.Core;
using CraftersCloud.Core.Tests.Shared;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Autofac;

[UsedImplicitly]
public class TestModule : Module
{
    protected override void Load(ContainerBuilder builder) =>
        // replace the default TimeProvider with a TestTimeProvider (allows us to control the current time in tests)
        builder.RegisterType<TestTimeProvider>()
            .As<ITimeProvider>()
            .As<TestTimeProvider>()
            .SingleInstance(); // singleton so that the same instance is used across the entire test run (first request sets the time, subsequent requests get the same time)
}