using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

public static class UserQueryableExtensions
{
    public static IQueryable<User> IncludeAggregate(this IQueryable<User> query) =>
        query
            .Include(u => u.UserStatus)
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions);

    public static IQueryable<User> QueryByEmail(this IQueryable<User> query, string email) =>
        query.Where(e => e.EmailAddress == email);

    public static IQueryable<User> QueryEmailOptional(this IQueryable<User> query, string? email,
        Func<string, string> pattern) =>
        !string.IsNullOrEmpty(email)
            ? query.Where(e => EF.Functions.Like(e.EmailAddress, pattern(email)))
            : query;

    public static IQueryable<User> QueryActiveOnly(this IQueryable<User> query) =>
        query.QueryByStatusOptional(UserStatusId.Active);

    public static IQueryable<User> QueryByStatusOptional(this IQueryable<User> query, UserStatusId? status) =>
        status != null ? query.Where(e => e.UserStatusId == status) : query;

    public static IQueryable<User> QueryByNameOptional(this IQueryable<User> query, string? name) =>
        !string.IsNullOrEmpty(name)
            ? query.Where(
                e => EF.Functions.Like(e.FirstName, $"%{name}%")
                     || EF.Functions.Like(e.LastName, $"%{name}%"))
            : query;
}