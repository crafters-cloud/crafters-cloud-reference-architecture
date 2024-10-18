using System.Security.Claims;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security.DummyAuthentication;

public static class DummyUserData
{
    public static ClaimsPrincipal CreateClaimsPrincipal() =>
        new(new ClaimsIdentity([new Claim(ClaimTypes.Upn, "N/A")], "DummyAuth"));
}