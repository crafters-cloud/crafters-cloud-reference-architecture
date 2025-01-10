using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

public static class ProductQueryableExtensions
{
    public static IQueryable<Product> IncludeAggregate(this IQueryable<Product> query) =>
        query;

    public static IQueryable<Product> QueryById(this IQueryable<Product> query, ProductId id) =>
        query.Where(e => e.Id == id);

    public static IQueryable<Product> QueryExceptWithId(this IQueryable<Product> query, ProductId? id)
        => query.Where(e => e.Id != id);

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