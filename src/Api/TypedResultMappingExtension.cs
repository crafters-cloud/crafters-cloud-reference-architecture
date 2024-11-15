namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Identity;

public static class TypedResultMappingExtension
{
    public static Results<Ok<TDestination>, NotFound> ToMappedTypedResults<TSource, TDestination>(this TSource? model,
        Func<TSource, TDestination> mapper)
        where TSource : class
        => model != null ? TypedResults.Ok(mapper(model)) : TypedResults.NotFound();
}