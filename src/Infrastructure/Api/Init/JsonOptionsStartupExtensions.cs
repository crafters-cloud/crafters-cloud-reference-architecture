using System.Reflection;
using System.Text.Json.Serialization;
using CraftersCloud.Core.SmartEnums.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using OneOf.Serialization.SystemTextJson;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class JsonOptionsStartupExtensions
{
    public static void AppConfigureHttpJsonOptions(this IServiceCollection services, Assembly[] extraAssemblies) =>
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.AppRegisterJsonConverters(extraAssemblies));

    public static void AppRegisterJsonConverters(this IList<JsonConverter> converters, Assembly[] extraAssemblies)
    {
        converters.AddCoreSmartEnumJsonConverters([..extraAssemblies, AssemblyFinder.DomainAssembly]);
        converters.Add(new OneOfJsonConverter());
        converters.Add(new OneOfBaseJsonConverter());
    }
}