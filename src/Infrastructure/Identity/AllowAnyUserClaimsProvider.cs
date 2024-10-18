namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Identity;

public class AllowAnyUserClaimsProvider : IClaimsProvider
{
    public bool IsAuthenticated => true;
    public string? Email => "N/A";
}