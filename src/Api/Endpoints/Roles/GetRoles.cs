using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Roles;

public static class GetRoles
{
    public class Response : ItemsResponse<KeyValuePairDto<Guid>>;

    [UsedImplicitly]
    public static async Task<Ok<Response>> Handle(IRepository<Role, RoleId> roleRepository,
        CancellationToken cancellationToken)
    {
        var items = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => new KeyValuePairDto<Guid> { Key = r.Id, Value = r.Name })
            .ToListAsync(cancellationToken);

        return items.ToMinimalApiResult<Guid, Response>();
    }
}