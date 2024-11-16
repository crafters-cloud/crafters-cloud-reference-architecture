using CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;

namespace CraftersCloud.ReferenceArchitecture.Api;

public static class EndpointConventionBuilderExtensions
{
    public static TBuilder RequirePermissions<TBuilder>(this TBuilder builder, params PermissionId[] permissionIds)
        where TBuilder : IEndpointConventionBuilder =>
        builder.RequireAuthorization(new UserHasPermissionAttribute(permissionIds));
}