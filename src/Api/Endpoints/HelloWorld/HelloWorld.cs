namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;

public static class HelloWorld
{
    [PublicAPI]
    public class Response;
    
    public static Ok<Response> Handle() => TypedResults.Ok(new Response());
}