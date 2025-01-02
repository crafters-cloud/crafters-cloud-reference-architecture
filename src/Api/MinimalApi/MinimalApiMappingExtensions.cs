namespace CraftersCloud.ReferenceArchitecture.Api.MinimalApi;

public static class MinimalApiMappingExtensions
{
    public static Results<Ok<TDestination>, NotFound> ToMappedMinimalApiResult<TSource, TDestination>(
        this TSource? model,
        Func<TSource, TDestination> mapper)
        where TSource : class
        => model != null ? TypedResults.Ok(mapper(model)) : TypedResults.NotFound();
}