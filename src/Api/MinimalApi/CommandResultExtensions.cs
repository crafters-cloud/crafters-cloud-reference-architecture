using CraftersCloud.Core.AspNetCore.Errors;
using CraftersCloud.ReferenceArchitecture.Core.CommandResults;

namespace CraftersCloud.ReferenceArchitecture.Api.MinimalApi;

public static class CommandResultExtensions
{
    public static Results<Created<T>, BadRequest<ValidationProblemDetails>> ToMinimalApiResult<T>(
        this CreateCommandResult<T> command, HttpContext httpContext, Func<T, string> createdUri) =>
        command.Match<Results<Created<T>, BadRequest<ValidationProblemDetails>>>(
            created => TypedResults.Created(createdUri(created.Value), created.Value),
            badRequest => TypedResults.BadRequest(httpContext.CreateValidationProblemDetails(badRequest.Errors)));

    public static Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>
        ToMinimalApiResult<T>(this UpdateCommandResult<T> command, HttpContext httpContext) =>
        command.Match<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>>(
            noContent => TypedResults.NoContent(),
            notFound => TypedResults.NotFound(),
            badRequest =>
                TypedResults.BadRequest(httpContext.CreateValidationProblemDetails(badRequest.Errors)));
}