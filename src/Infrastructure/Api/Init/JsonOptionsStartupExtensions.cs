using System.Text.Json.Serialization;
using CraftersCloud.Core.SmartEnums.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class JsonOptionsStartupExtensions
{
    public static void AppConfigureHttpJsonOptions(this IServiceCollection services) =>
        services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.AppRegisterJsonConverters());

    public static void AppRegisterJsonConverters(this IList<JsonConverter> converters) =>
        converters.AddCoreSmartEnumJsonConverters([AssemblyFinder.ApiAssembly, AssemblyFinder.DomainAssembly]);
}