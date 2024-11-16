namespace CraftersCloud.ReferenceArchitecture.Api.Models;

[PublicAPI]
public class KeyValuePairDto<T>
{
    public required T Key { get; set; }
    public string Value { get; set; } = string.Empty;
}