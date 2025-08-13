using System.Reflection;
using Argon;
using CraftersCloud.Core.Tests.Shared.SmartEnums;
using CraftersCloud.Core.Tests.Shared.StronglyTypedIds;
using CraftersCloud.ReferenceArchitecture.Infrastructure;

namespace CraftersCloud.ReferenceArchitecture.Tests.Shared;

public static class CommonVerifierSettingsInitializer
{
    public static void Init(Assembly entryAssembly)
    {
        // properties marked [Obsolete] will also appear in the verified output
        VerifierSettings.IncludeObsoletes();

        VerifierSettings.AddExtraSettings(settings =>
        {
            // needed for SmartEnum serialization 
            settings.Converters.AddCoreVerifyTestsSmartEnumJsonConverters([entryAssembly, AssemblyFinder.DomainAssembly]);
            settings.Converters.AddCoreVerifyTestsStronglyTypedIdsJsonConverters([entryAssembly, AssemblyFinder.DomainAssembly]);

            settings.NullValueHandling = NullValueHandling.Ignore;

            // primarily for default enum values to appear in the verified output
            settings.DefaultValueHandling = DefaultValueHandling.Include;
        });
    }
}