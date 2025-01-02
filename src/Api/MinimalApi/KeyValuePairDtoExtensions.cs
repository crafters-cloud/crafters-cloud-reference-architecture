using CraftersCloud.ReferenceArchitecture.Api.Models;

namespace CraftersCloud.ReferenceArchitecture.Api.MinimalApi;

public static class KeyValuePairDtoExtensions
{
    public static Ok<T> ToMinimalApiResult<TKey, T>(this List<KeyValuePairDto<TKey>> items)
        where T : ItemsResponse<KeyValuePairDto<TKey>>, new() =>
        TypedResults.Ok(new T { Items = items });
}