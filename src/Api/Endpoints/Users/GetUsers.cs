using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class GetUsers
{
    [PublicAPI]
    public class Request : PagedQuery<Response.Item>
    {
        [FromQuery] public string? Name { get; set; }
        [FromQuery] public string? EmailAddress { get; set; }
    }

    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            this.AddPagedQueryRules();
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.EmailAddress).MaximumLength(100);
        }
    }

    [PublicAPI]
    public class Response
    {
        [PublicAPI]
        public class Item
        {
            public UserId Id { get; set; }
            public string EmailAddress { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string UserStatusName { get; set; } = string.Empty;
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
        }
    }

    [Mapper]
    public static partial class Mapper
    {
        public static partial IQueryable<Response.Item> Map(IQueryable<User> q);
    }

    public static async Task<Ok<PagedQueryResponse<Response.Item>>> Handle([AsParameters] Request request,
        IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        var query = repository.QueryAll()
            .Include(u => u.UserStatus)
            .AsNoTracking()
            .QueryActiveOnly()
            .QueryByNameOptional(request.Name)
            .QueryEmailOptional(request.EmailAddress, SearchPatterns.Like);

        var items = await Mapper.Map(query)
            .ToPagedResponseAsync(request, cancellationToken);

        return TypedResults.Ok(items);
    }
}