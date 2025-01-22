using OneOf;
using NotFoundResult = CraftersCloud.Core.Results.NotFoundResult;

namespace CraftersCloud.ReferenceArchitecture.Api.MinimalApi;

public static class MinimalApiMappingExtensions
{
    public static Results<Ok<TDestination>, NotFound> ToMinimalApiResult<TSource, TDestination>(
        this OneOf<TSource, NotFoundResult> result,
        Func<TSource, TDestination> mapper)
        where TSource : class
        => result.Match<Results<Ok<TDestination>, NotFound>>(
            model => TypedResults.Ok(mapper(model)),
            _ => TypedResults.NotFound());

    public static Results<Ok<TSource>, NotFound> ToMinimalApiResult<TSource>(
        this TSource? model)
        where TSource : class
        => model.ToMinimalApiResult(x => x);

    public static Results<Ok<TDestination>, NotFound> ToMinimalApiResult<TSource, TDestination>(
        this TSource? model,
        Func<TSource, TDestination> mapper)
        where TSource : class
        => model != null ? TypedResults.Ok(mapper(model)) : TypedResults.NotFound();
}