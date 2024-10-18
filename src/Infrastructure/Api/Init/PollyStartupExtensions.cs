using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Timeout;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class PollyStartupExtensions
{
    private const string GlobalTimeoutPolicyName = "global-timeout";

    public static void AppAddPolly(this IServiceCollection services)
    {
        // Add registry
        var policyRegistry = services.AddPolicyRegistry();

        // Centrally stored policies
        var timeoutPolicy =
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));
        policyRegistry.Add(GlobalTimeoutPolicyName, timeoutPolicy);
    }
}