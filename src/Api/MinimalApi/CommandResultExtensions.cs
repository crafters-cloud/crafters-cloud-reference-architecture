using CraftersCloud.Core.AspNetCore;
using CraftersCloud.ReferenceArchitecture.Core.CommandResults;

namespace CraftersCloud.ReferenceArchitecture.Api.MinimalApi;

public static class CommandResultExtensions
{
    public static Results<Created<T>, BadRequest<ValidationProblemDetails>> ToMinimalApiResult<T>(
        this CreateCommandResult<T> command, Func<T, string> createdUri) =>
        command.Match<Results<Created<T>, BadRequest<ValidationProblemDetails>>>(
            created => TypedResults.Created(createdUri(created.Value), created.Value),
            badRequest => TypedResults.BadRequest(ValidationProblemDetailsMapper.CreateValidationProblemDetails(
                string.Empty,
                badRequest.Errors)));

    public static Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>
        ToMinimalApiResult<T>(this UpdateCommandResult<T> command) =>
        command.Match<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>>(
            noContent => TypedResults.NoContent(),
            notFound => TypedResults.NotFound(),
            badRequest =>
                TypedResults.BadRequest(
                    ValidationProblemDetailsMapper.CreateValidationProblemDetails(string.Empty,
                        badRequest.Errors)));
}