using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetStatuses
{
    [PublicAPI]
    public class Request : LookupRequest<UserStatusId>;

    [PublicAPI]
    public class Response : LookupResponse<UserStatusId>;

    [UsedImplicitly]
    public class RequestHandler(IRepository<UserStatus, UserStatusId> roleRepository)
        : IRequestHandler<Request, IEnumerable<LookupResponse<UserStatusId>>>
    {
        public async Task<IEnumerable<LookupResponse<UserStatusId>>> Handle(Request request,
            CancellationToken cancellationToken) =>
            await roleRepository
                .QueryAll()
                .Select(x => new Response { Value = x.Id, Label = x.Name })
                .OrderBy(r => r.Label)
                .ToListAsync(cancellationToken);
    }
}