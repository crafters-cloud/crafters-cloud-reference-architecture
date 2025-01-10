using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Data.Configurations;

public static class PropertyBuilderExtensions
{
    public static void HasStronglyTypedId<TId, TValue>(this PropertyBuilder<TId> propertyBuilder,
        Expression<Func<TId, TValue>> convertToProviderExpression,
        Expression<Func<TValue, TId>> convertFromProviderExpression) =>
        propertyBuilder
            .HasConversion(new ValueConverter<TId, TValue>(convertToProviderExpression, convertFromProviderExpression))
            .ValueGeneratedNever();
}