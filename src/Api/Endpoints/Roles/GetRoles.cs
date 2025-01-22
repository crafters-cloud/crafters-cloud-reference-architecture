using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Roles;

public static class GetRoles
{
    public class Response : ItemsResponse<KeyValuePairDto<RoleId>>;

    [UsedImplicitly]
    public static async Task<Ok<Response>> Handle(IRepository<Role> roleRepository,
        CancellationToken cancellationToken)
    {
        var items = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => new KeyValuePairDto<RoleId> { Key = r.Id, Value = r.Name })
            .ToListAsync(cancellationToken);

        return items.ToMinimalApiResult<RoleId, Response>();
    }
}