using Ardalis.Result;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class ResultMappingExtensions
{
    public static Result<TDestination> ToMappedResult<TSource, TDestination>(this TSource? source,
        Func<TSource, TDestination> mapper) where TSource : class where TDestination : class =>
        source != null ? Result.Success(mapper(source)) : Result.NotFound();
}