using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetUserDetails
{
    [PublicAPI]
    public class Request : IQuery<Response>
    {
        public Guid Id { get; set; }

        public static Request ById(Guid id) => new() { Id = id };
    }

    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;
        public string UserStatusName { get; set; } = string.Empty;
        public string UserStatusDescription { get; set; } = string.Empty;
    }

    [UsedImplicitly]
    public class RequestHandler(IRepository<User> repository) : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await repository.QueryAll()
                .Include(x => x.UserStatus)
                .AsNoTracking()
                .QueryById(request.Id)
                .SingleOrNotFoundAsync(cancellationToken);

            return user.ToResponse();
        }
    }
}