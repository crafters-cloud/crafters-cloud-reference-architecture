using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Identity;

public static class UserQueryableExtensions
{
    public static IQueryable<User> BuildAggregateInclude(this IQueryable<User> query) =>
        query
            .Include(u => u.UserStatus)
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions);

    public static IQueryable<User> QueryByName(this IQueryable<User> query, string? name) =>
        !string.IsNullOrEmpty(name)
            ? query.Where(e => e.FullName.Contains(name))
            : query;

    public static IQueryable<User> QueryByEmail(this IQueryable<User> query, string? email) =>
        !string.IsNullOrEmpty(email)
            ? query.Where(e => e.EmailAddress.Contains(email))
            : query;
}