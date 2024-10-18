using System.Reflection;
using Argon;
using CraftersCloud.Core.SmartEnums.VerifyTests;
using CraftersCloud.ReferenceArchitecture.Infrastructure;

namespace CraftersCloud.ReferenceArchitecture.Common.Tests;

public static class CommonVerifierSettingsInitializer
{
    public static void Init(Assembly entryAssembly)
    {
        // properties marked [Obsolete] will also appear in the verified output
        VerifierSettings.IncludeObsoletes();

        VerifierSettings.AddExtraSettings(settings =>
        {
            // needed for SmartEnum serialization 
            settings.Converters.AddCoreSmartEnumJsonConverters([entryAssembly, AssemblyFinder.DomainAssembly]);

            settings.NullValueHandling = NullValueHandling.Ignore;

            // primarily for default enum values to appear in the verified output
            settings.DefaultValueHandling = DefaultValueHandling.Include;
        });
    }
}