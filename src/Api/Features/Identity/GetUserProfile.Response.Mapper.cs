using CraftersCloud.ReferenceArchitecture.Api.Features.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Authorization;

[Mapper]
public static partial class GetUserProfileResponseMapper
{
    [MapperIgnoreTarget(nameof(GetUserProfile.Response.Permissions))]
    private static partial GetUserProfile.Response MapToResponse(User source);
    
    [UserMapping(Default = true)]
    public static GetUserProfile.Response ToResponse(this User source)
    {
        var dto = MapToResponse(source);
        dto.Permissions = source.GetPermissionIds();
        return dto;
    }
    
}