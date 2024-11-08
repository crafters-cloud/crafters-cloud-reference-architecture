using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Authorization;

public static class GetUserProfile
{
    [PublicAPI]
    public class Request : IQuery<Response>;

    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public IReadOnlyCollection<PermissionId> Permissions { get; set; } = [];
    }

    [UsedImplicitly]
    public class RequestHandler(ICurrentUserProvider currentUserProvider, IRepository<User> repository)
        : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            if (currentUserProvider.UserId is null)
            {
                throw new InvalidOperationException(
                    "UserId could not be determined. This could happen if user is authenticated but it could not be found in the database.");
            }

            var user = await repository.QueryAll().QueryById(currentUserProvider.UserId.Value)
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .Include(u => u.UserStatus)
                .AsNoTracking()
                .SingleOrNotFoundAsync(cancellationToken);

            return user.ToResponse();
        }
    }
}