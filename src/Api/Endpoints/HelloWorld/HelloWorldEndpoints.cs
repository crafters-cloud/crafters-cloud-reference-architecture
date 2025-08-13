namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.HelloWorld;

[UsedImplicitly]
public class HelloWorldEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hello-world")
            .WithGroupName("Hello World");

        group.MapGet("/", HelloWorld.Handle);
    }
}