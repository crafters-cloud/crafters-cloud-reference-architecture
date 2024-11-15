namespace CraftersCloud.ReferenceArchitecture.Core;

[PublicAPI]
public class KeyValuePair<T>
{
    public required T Key { get; set; }
    public string Value { get; set; } = string.Empty;
}