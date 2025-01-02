namespace CraftersCloud.ReferenceArchitecture.Api.Models;

[PublicAPI]
public class KeyValuePairDto<TKey>
{
    public required TKey Key { get; set; }
    public string Value { get; set; } = string.Empty;
}