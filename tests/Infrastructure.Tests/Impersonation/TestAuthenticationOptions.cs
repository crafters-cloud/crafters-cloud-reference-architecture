using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.Impersonation;

public class TestAuthenticationOptions : AuthenticationSchemeOptions
{
    public Func<ClaimsPrincipal?> TestPrincipalFactory { get; set; } = () => null;
}