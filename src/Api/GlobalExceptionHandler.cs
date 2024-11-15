using System.Diagnostics;
using CraftersCloud.Core.AspNetCore.Filters;
using CraftersCloud.Core.AspNetCore.Validation;
using Microsoft.AspNetCore.Diagnostics;

namespace CraftersCloud.ReferenceArchitecture.Api;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        switch (exception)
        {
            case ValidationException validationException:
                var validationProblemDetails = httpContext.CreateValidationProblemDetails(validationException);
                await WriteProblemDetails(validationProblemDetails, httpContext, cancellationToken);
                break;
            default:
                var problemDetails = GetProblemDetails(httpContext, exception);
                await WriteProblemDetails(problemDetails, httpContext, cancellationToken);
                break;
        }

        return true;
    }

    private static async Task WriteProblemDetails<T>(T problemDetails, HttpContext httpContext, CancellationToken cancellationToken) where T : ProblemDetails
    {
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private static ProblemDetails GetProblemDetails(HttpContext context, Exception exception)
    {
        var environment = context.Resolve<IHostEnvironment>();
        var errorDetail = environment.IsDevelopment()
            ? exception.Demystify().ToString()
            : "The instance value should be used to identify the problem when calling customer support";

        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred!",
            Instance = context.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Detail = errorDetail
        };

        return problemDetails;
    }
}