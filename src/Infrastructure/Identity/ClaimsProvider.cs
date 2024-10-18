using System.Security.Claims;
using System.Security.Principal;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;

public class ClaimsProvider(Func<IPrincipal> principalProvider) : IClaimsProvider
{
    private static readonly string[] EmailClaims =
    {
        ClaimTypes.Upn, // used in AzureAD 1.0 tokens
        "preferred_username", // used in AzureAD 2.0 tokens
        "emails", // used in AzureAD B2C tokens
        ClaimTypes.Email
    };

    private ClaimsPrincipal? Principal => principalProvider() as ClaimsPrincipal;
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public string? Email => Principal?.Identity is not ClaimsIdentity identity
        ? null
        : identity.Claims.FirstOrDefault(claim => EmailClaims.Contains(claim.Type))?.Value;
}