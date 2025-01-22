using System.Reflection;
using System.Text.Json.Serialization;
using CraftersCloud.Core.SmartEnums.SystemTextJson;
using CraftersCloud.Core.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using OneOf.Serialization.SystemTextJson;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class JsonOptionsStartupExtensions
{
    public static void AppConfigureHttpJsonOptions(this IServiceCollection services, Assembly entryAssembly) =>
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.AppRegisterJsonConverters(entryAssembly));

    public static void AppRegisterJsonConverters(this IList<JsonConverter> converters, Assembly entryAssembly)
    {
        converters.AddCoreSmartEnumJsonConverters([entryAssembly, AssemblyFinder.DomainAssembly]);
        converters.AddCoreStronglyTypedIdsJsonConverters([entryAssembly, AssemblyFinder.DomainAssembly]);
        converters.Add(new OneOfJsonConverter());
        converters.Add(new OneOfBaseJsonConverter());
    }
}