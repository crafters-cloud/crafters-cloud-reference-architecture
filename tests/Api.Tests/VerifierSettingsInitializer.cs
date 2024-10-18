using System.Runtime.CompilerServices;
using CraftersCloud.ReferenceArchitecture.Common.Tests;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests;

public static class VerifierSettingsInitializer
{
    [ModuleInitializer]
    public static void Init() => CommonVerifierSettingsInitializer.Init(AssemblyFinder.ApiAssembly);
}