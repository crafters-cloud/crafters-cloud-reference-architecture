using System.Runtime.CompilerServices;
using CraftersCloud.ReferenceArchitecture.Infrastructure;
using CraftersCloud.ReferenceArchitecture.Tests.Shared;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests;

public static class VerifierSettingsInitializer
{
    [ModuleInitializer]
    public static void Init() => CommonVerifierSettingsInitializer.Init(AssemblyFinder.ApiAssembly);
}