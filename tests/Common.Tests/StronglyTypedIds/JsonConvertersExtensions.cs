using System.Reflection;
using Argon;
using CraftersCloud.ReferenceArchitecture.Core;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Common.Tests.StronglyTypedIds;

public static class JsonConvertersExtensions
{
    public static void AddStronglyTypedIdsJsonConverters(this IList<JsonConverter> converters,
        IEnumerable<Assembly> assemblies)
    {
        var type1 = typeof(UserId);
        var blah = type1.IsStronglyTypedId();
        var stronglyTypedIdsTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsStronglyTypedId());

        foreach (var type in stronglyTypedIdsTypes)
        {
            var converterType =
                typeof(StronglyTypedIdWriteOnlyJsonConverter<>).MakeGenericType(type);
            var converter = (JsonConverter) Activator.CreateInstance(converterType)!;
            converters.Add(converter);
        }
    }

    private static bool IsStronglyTypedId(this Type objectType) => objectType.GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStronglyTypedId<>));
}