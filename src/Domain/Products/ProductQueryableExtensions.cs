using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

public static class ProductQueryableExtensions
{
    // Use this method to build Root Aggregate when loading
    public static IQueryable<Product> IncludeAggregate(this IQueryable<Product> query) =>
        query;

    public static IQueryable<Product> QueryByName(this IQueryable<Product> query, string email) =>
        query.Where(e => e.Name == email);

    public static IQueryable<Product> QueryByNameOptional(this IQueryable<Product> query, string? name) =>
        !string.IsNullOrEmpty(name)
            ? query.Where(e => EF.Functions.Like(e.Name, $"%{name}%"))
            : query;

    public static IQueryable<Product> QueryActiveOnly(this IQueryable<Product> query) =>
        query.QueryByStatusOptional(ProductStatusId.Active);

    public static IQueryable<Product> QueryByStatusOptional(this IQueryable<Product> query, ProductStatusId? status) =>
        status != null ? query.Where(e => e.ProductStatusId == status) : query;
}