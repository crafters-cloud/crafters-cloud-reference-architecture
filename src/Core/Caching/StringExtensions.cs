
namespace CraftersCloud.ReferenceArchitecture.Core.Caching;

public static class StringExtensions
{
    public static string RemoveCharactersNotSuitableForCacheKey(this string value) => value.Replace('.', ':');

    public static string RemoveEverythingAfterInclusive(this string value, char after)
    {
        var index = value.IndexOf(after);
        return index >= 0 ? value.Remove(index) : value;
    }

    public static string RemoveAtBeginning(this string value, string toRemove) =>
        value.StartsWith(toRemove)
            ? value[toRemove.Length..]
            : value;
}