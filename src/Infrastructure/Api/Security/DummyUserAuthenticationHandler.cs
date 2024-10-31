using System.Security.Claims;
using System.Text.Encodings.Web;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security.DummyAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

public class DummyUserAuthenticationHandler(
    IOptionsMonitor<DummyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<DummyAuthenticationOptions>(options, logger, encoder)
{
    public const string AuthenticationScheme = "DummyUserAuth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var principal = DummyUserData.CreateClaimsPrincipal();
        var authResult = AuthenticatedUserResult(principal);
        return Task.FromResult(authResult);
    }

    private static AuthenticateResult AuthenticatedUserResult(ClaimsPrincipal testPrincipal) =>
        AuthenticateResult.Success(new AuthenticationTicket(testPrincipal, AuthenticationScheme));
}