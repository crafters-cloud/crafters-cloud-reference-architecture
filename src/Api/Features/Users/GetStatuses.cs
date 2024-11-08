using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetStatuses
{
    [PublicAPI]
    public class ResponseItem : LookupResponse<UserStatusId>;

    public static async Task<Ok<List<ResponseItem>>> Handle(
        IRepository<UserStatus, UserStatusId> roleRepository,
        CancellationToken cancellationToken)
    {
        var userStatuses = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .Select(x => new ResponseItem { Value = x.Id, Label = x.Name })
            .OrderBy(r => r.Label)
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(userStatuses);
    }
}