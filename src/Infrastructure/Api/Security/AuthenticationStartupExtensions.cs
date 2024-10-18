using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security.DummyAuthentication;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

public static class AuthenticationStartupExtensions
{
    public static void AppAddAuthentication(this IServiceCollection services) =>
        services.AddAuthentication(DummyUserAuthenticationHandler.AuthenticationScheme)
            .AddScheme<DummyAuthenticationOptions, DummyUserAuthenticationHandler>(
                DummyUserAuthenticationHandler.AuthenticationScheme,
                _ => { });
}