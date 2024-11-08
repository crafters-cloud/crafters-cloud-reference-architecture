using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Riok.Mapperly.Abstractions;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

[Mapper]
public static partial class GetUsersResponseItemMapper
{
    public static partial IQueryable<GetUsers.Response.Item> ProjectToResponse(this IQueryable<User> q);
}