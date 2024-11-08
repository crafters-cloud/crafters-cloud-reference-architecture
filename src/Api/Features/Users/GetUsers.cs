using CraftersCloud.Core.Data;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetUsers
{
    [PublicAPI]
    public class Request : PagedRequest<Response.Item>
    {
        [FromQuery]
        public string? Name { get; set; }
        [FromQuery]
        public string? Email { get; set; }
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

    public static async Task<Ok<PagedResponse<Response.Item>>> Handle([AsParameters]Request request,
        IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        var items = await repository.QueryAll()
            .Include(u => u.UserStatus)
            .AsNoTracking()
            .QueryByName(request.Name)
            .QueryByEmail(request.Email)
            .ProjectToResponse()
            .ToPagedResponseAsync(request, cancellationToken);

        return TypedResults.Ok(items);
    }
}