namespace CraftersCloud.ReferenceArchitecture.Domain;

public static class SearchPatterns
{
    public static readonly Func<string, string> Like = value => $"%{value}%";
}