﻿using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static class GetUserStatuses
{
    [PublicAPI]
    public class Response : ItemsResponse<KeyValuePairDto<int>>;

    public static async Task<Ok<Response>> Handle(
        IRepository<UserStatus, UserStatusId> repository,
        CancellationToken cancellationToken)
    {
        var items = await repository
            .QueryAll()
            .AsNoTracking()
            .Select(x => new KeyValuePairDto<int> { Key = x.Id, Value = x.Name })
            .OrderBy(r => r.Value)
            .ToListAsync(cancellationToken);

        return items.ToMinimalApiResult<int, Response>();
    }
}