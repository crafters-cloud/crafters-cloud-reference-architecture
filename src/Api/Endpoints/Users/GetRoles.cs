using CraftersCloud.ReferenceArchitecture.Core;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class GetRoles
{
    [PublicAPI]
    public class ResponseItem : KeyValuePair<Guid>;

    [UsedImplicitly]
    public static async Task<Ok<List<ResponseItem>>> Handle(IRepository<Role> roleRepository,
        CancellationToken cancellationToken)
    {
        var roles = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => new ResponseItem { Key = r.Id, Value = r.Name })
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(roles);
    }
}