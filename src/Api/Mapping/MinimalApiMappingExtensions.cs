using CraftersCloud.ReferenceArchitecture.Api.Models;

namespace CraftersCloud.ReferenceArchitecture.Api.Mapping;

public static class MinimalApiMappingExtensions
{
    public static Results<Ok<TDestination>, NotFound> ToMappedMinimalApiResult<TSource, TDestination>(
        this TSource? model,
        Func<TSource, TDestination> mapper)
        where TSource : class
        => model != null ? TypedResults.Ok(mapper(model)) : TypedResults.NotFound();

    public static Ok<T> ToMinimalApiResult<T>(this List<KeyValuePairDto<int>> items)
        where T : ItemsListResponse<KeyValuePairDto<int>>, new() =>
        TypedResults.Ok(ItemsListResponse.Create<T, KeyValuePairDto<int>>(items));


    public static Ok<T> ToMinimalApiResult<T>(this List<KeyValuePairDto<Guid>> items)
        where T : ItemsListResponse<KeyValuePairDto<Guid>>, new() =>
        TypedResults.Ok(ItemsListResponse.Create<T, KeyValuePairDto<Guid>>(items));
}