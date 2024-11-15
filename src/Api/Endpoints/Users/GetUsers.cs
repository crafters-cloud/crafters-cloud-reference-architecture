using CraftersCloud.Core.Paging;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class GetUsers
{
    [PublicAPI]
    public class Request : PagedRequest<Response.Item>
    {
        [FromQuery] public string? Name { get; set; }
        [FromQuery] public string? Email { get; set; }
    }

    [UsedImplicitly]
    public class RequestValidator : PagedRequestValidator<Request, Response.Item>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.Email).MaximumLength(100);
        }
    }

    [PublicAPI]
    public class Response
    {
        [PublicAPI]
        public class Item
        {
            public Guid Id { get; set; }
            public string EmailAddress { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string UserStatusName { get; set; } = string.Empty;
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
        }
    }


    [Mapper]
    public static partial class ResponseItemQueryMapper
    {
        public static partial IQueryable<Response.Item> ProjectTo(IQueryable<User> q);
    }

    public static async Task<Ok<PagedResponse<Response.Item>>> Handle([AsParameters] Request request,
        IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        var query = repository.QueryAll()
            .Include(u => u.UserStatus)
            .AsNoTracking()
            .QueryByName(request.Name)
            .QueryByEmail(request.Email);

        var items = await ResponseItemQueryMapper.ProjectTo(query)
            .ToPagedResponseAsync(request, cancellationToken);

        return TypedResults.Ok(items);
    }
}