using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.CreateUser;

[UsedImplicitly]
public static partial class CreateUser
{
    public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, ProblemDetails>>
    {
        public override void Configure()
        {
            Post("/api/user/create");
            AllowAnonymous();
        }

        public override async Task<Results<Ok<Response>, NotFound, ProblemDetails>> ExecuteAsync(Request req,
            CancellationToken ct)
        {
            await Task.CompletedTask; //simulate async work

            if (req.Id == 0) //condition for a not found response
            {
                return TypedResults.NotFound();
            }

            if (req.Id == 1) //condition for a problem details response
            {
                AddError(r => r.Id, "value has to be greater than 1");
                return new ProblemDetails(ValidationFailures);
            }

            // 200 ok response with a DTO
            return TypedResults.Ok(new Response
            {
                RequestedId = req.Id
            });
        }
    }
}