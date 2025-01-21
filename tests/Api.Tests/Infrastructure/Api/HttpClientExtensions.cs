using System.Text.Json;
using CraftersCloud.ReferenceArchitecture.Infrastructure;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;
using Flurl.Http.Configuration;

namespace CraftersCloud.ReferenceArchitecture.Api.Tests.Infrastructure.Api;

internal static class HttpClientExtensions
{
    internal static FlurlClient ToFlurlClient(this HttpClient httpClient) =>
        new FlurlClient(httpClient).WithSettings(settings =>
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.Converters.AppRegisterJsonConverters([AssemblyFinder.ApiAssembly]);
            settings.JsonSerializer = new DefaultJsonSerializer(options);
        });
}