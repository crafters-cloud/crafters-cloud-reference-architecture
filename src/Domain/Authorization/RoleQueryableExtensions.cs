﻿namespace CraftersCloud.ReferenceArchitecture.Domain.Authorization;

public static class RoleQueryableExtensions
{
    public static IQueryable<Role> QueryByName(this IQueryable<Role> query, string name) =>
        query.Where(x => x.Name == name);
}