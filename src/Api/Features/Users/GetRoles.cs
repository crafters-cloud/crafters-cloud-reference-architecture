using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetRoles
{
    [PublicAPI]
    public class Request : LookupRequest<Guid>;

    [PublicAPI]
    public class Response : LookupResponse<Guid>;

    [UsedImplicitly]
    public class RequestHandler(IRepository<Role> roleRepository)
        : IRequestHandler<Request, IEnumerable<LookupResponse<Guid>>>
    {
        public async Task<IEnumerable<LookupResponse<Guid>>> Handle(Request request,
            CancellationToken cancellationToken)
        {
            var roles = await roleRepository
                .QueryAll()
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .ToListAsync(cancellationToken);

            return roles.Select(r => new Response { Value = r.Id, Label = r.Name });
        }
    }
}