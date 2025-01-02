using CraftersCloud.ReferenceArchitecture.Api.Mapping;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class GetStatuses
{
    [PublicAPI]
    public class Response : ItemsListResponse<KeyValuePairDto<int>>;

    public static async Task<Ok<Response>> Handle(
        IRepository<UserStatus, UserStatusId> roleRepository,
        CancellationToken cancellationToken)
    {
        var items = await roleRepository
            .QueryAll()
            .AsNoTracking()
            .Select(x => new KeyValuePairDto<int> { Key = x.Id, Value = x.Name })
            .OrderBy(r => r.Value)
            .ToListAsync(cancellationToken);

        return items.ToMinimalApiResult<Response>();
    }
}