using CraftersCloud.Core.AspNetCore.Authorization.Attributes;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;

public sealed class UserHasPermissionAttribute(params PermissionId[] permissions)
    : UserHasPermissionAttribute<PermissionId>(permissions);