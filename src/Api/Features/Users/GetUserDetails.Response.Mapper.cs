using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

[Mapper]
public static partial class GetUsersDetailsResponseMapper
{
    public static partial GetUserDetails.Response ToResponse(this User source);
}