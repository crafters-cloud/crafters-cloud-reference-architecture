namespace CraftersCloud.ReferenceArchitecture.Api.MinimalApi;

public static class MinimalApiMappingExtensions
{
    public static Results<Ok<TSource>, NotFound> ToMinimalApiResult<TSource>(
        this TSource? model)
        where TSource : class
        => model != null ? TypedResults.Ok(model) : TypedResults.NotFound();
    
    public static Results<Ok<TDestination>, NotFound> ToMinimalApiResult<TSource, TDestination>(
        this TSource? model,
        Func<TSource, TDestination> mapper)
        where TSource : class
        => model != null ? TypedResults.Ok(mapper(model)) : TypedResults.NotFound();
}