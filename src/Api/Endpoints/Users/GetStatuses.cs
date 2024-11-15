using CraftersCloud.ReferenceArchitecture.Core;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class GetStatuses
{
    [PublicAPI]
    public class ResponseItem : KeyValuePair<UserStatusId>;

    public static async Task<Ok<List<ResponseItem>>> Handle(
        IRepository<UserStatus, UserStatusId> roleRepository,
        CancellationToken cancellationToken)
    {
        var userStatuses = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .Select(x => new ResponseItem { Key = x.Id, Value = x.Name })
            .OrderBy(r => r.Value)
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(userStatuses);
    }
}