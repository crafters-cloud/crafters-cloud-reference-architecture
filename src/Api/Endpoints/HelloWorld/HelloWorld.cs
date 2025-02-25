using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Application.Identity.GetUserById;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;

public static class HelloWorld
{
    [PublicAPI]
    public class Response
    {
        public required string UserFullName { get; init; } = string.Empty;
    };
    
    public static async Task<Results<Ok<Response>, NotFound>> Handle(UserId? id, ISender sender, ILogger<HelloWorldEndpoints> logger,
        CancellationToken cancellationToken)
    {
        var query = new Query(id ?? UserId.SystemUserId);
        var response = await sender.Send(query, cancellationToken);
        return response.ToMinimalApiResult(x => new Response { UserFullName = x.FirstName + x.LastName });
    }
}