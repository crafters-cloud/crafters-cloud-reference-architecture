﻿using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Api.Models;
using CraftersCloud.ReferenceArchitecture.Domain.Products;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Products;

public static class GetProductStatuses
{
    [PublicAPI]
    public class Response : ItemsResponse<KeyValuePairDto<ProductStatusId>>;

    public static async Task<Ok<Response>> Handle(
        IRepository<ProductStatus> repository,
        CancellationToken cancellationToken)
    {
        var items = await repository
            .QueryAll()
            .AsNoTracking()
            .Select(x => new KeyValuePairDto<ProductStatusId> { Key = x.Id, Value = x.Name })
            .OrderBy(r => r.Value)
            .ToListAsync(cancellationToken);

        return items.ToMinimalApiResult<ProductStatusId, Response>();
    }
}