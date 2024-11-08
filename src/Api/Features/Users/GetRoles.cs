using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetRoles
{
    [PublicAPI]
    public class ResponseItem : LookupResponse<Guid>;

    [UsedImplicitly]
    public static async Task<Ok<List<ResponseItem>>> Handle(IRepository<Role> roleRepository,
        CancellationToken cancellationToken)
    {
        var roles = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => new ResponseItem { Value = r.Id, Label = r.Name })
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(roles);
    }
}