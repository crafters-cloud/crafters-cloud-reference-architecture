using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;
using Microsoft.AspNetCore.Builder;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Security;

public static class EndpointConventionBuilderExtensions
{
    public static TBuilder RequirePermissions<TBuilder>(this TBuilder builder, params PermissionId[] permissionIds)
        where TBuilder : IEndpointConventionBuilder =>
        builder.RequireAuthorization(new UserHasPermissionAttribute(permissionIds));
}