using System.Reflection;
using CraftersCloud.ReferenceArchitecture.Core.Caching;

namespace Txf.VirtualEvents.Common.Tests.MediatR;

public static class TypeExtensions
{
    public static bool IsConcrete(this Type type)
    {
        if (!type.GetTypeInfo().IsAbstract)
        {
            return !type.GetTypeInfo().IsInterface;
        }

        return false;
    }

    public static IEnumerable<Type> FilterDependingOnTypes(this Type dependent, params Type[] candidates) =>
        candidates.Where(dependent.DependsOn);

    private static bool DependsOn(this Type dependent, Type candidate)
    {
        foreach (ConstructorInfo constructor in dependent.GetConstructors())
        foreach (ParameterInfo parameter in constructor.GetParameters())
            if (candidate.IsAssignableFrom(parameter.ParameterType))
            {
                return true;
            }

        return false;
    }

    public static IEnumerable<TypeInfo> FindCacheEvictors(this Type holdingNestedCacheEvictorType,
        bool lookupBaseTypes)
    {
        IEnumerable<TypeInfo> nestedTypes = lookupBaseTypes
            ? holdingNestedCacheEvictorType.AllBaseTypes()
                .SelectMany(baseType => baseType.GetTypeInfo().DeclaredNestedTypes)
            : holdingNestedCacheEvictorType.GetTypeInfo().DeclaredNestedTypes;

        return FindCacheEvictorTypes(nestedTypes);
    }
    
    private static IEnumerable<TypeInfo> FindCacheEvictorTypes(IEnumerable<TypeInfo> nestedTypes)
    {
        IEnumerable<TypeInfo> cacheEvictors =
            nestedTypes.Where(nestedType => typeof(CacheEvictorsBase).IsAssignableFrom(nestedType)).ToList();
        return cacheEvictors;
    }

    private static IEnumerable<Type> AllBaseTypes(this Type type)
    {
        var current = type;
        while (current != null)
        {
            yield return current;
            current = current.BaseType;
        }
    }
}