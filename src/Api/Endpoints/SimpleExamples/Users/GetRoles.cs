using CraftersCloud.ReferenceArchitecture.Api.Mapping;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.SimpleExamples.Users;

public static class GetRoles
{
    public class Response : ItemsListResponse<KeyValuePairDto<Guid>>;

    [UsedImplicitly]
    public static async Task<Ok<Response>> Handle(IRepository<Role> roleRepository,
        CancellationToken cancellationToken)
    {
        var items = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .Select(r => new KeyValuePairDto<Guid> { Key = r.Id, Value = r.Name })
            .ToListAsync(cancellationToken);

        return items.ToMinimalApiResult<Response>();
    }
}