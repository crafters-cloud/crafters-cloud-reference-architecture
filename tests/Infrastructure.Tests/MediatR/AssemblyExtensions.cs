using System.Reflection;
using CraftersCloud.Core.Caching.Abstractions;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.MediatR;

public static class AssemblyExtensions
{
    public static IEnumerable<RequestHandlerMeta>
        FindAllRequestsAndRequestHandlersThatAreCached(this Assembly assembly) =>
        assembly.FindAllRequestHandlersAndRequestTypes()
            .FilterHandlersForCachedRequest();

    private static IEnumerable<RequestHandlerMeta> FindAllRequestHandlersAndRequestTypes(
        this Assembly assembly) =>
        assembly.FindAllMediatRRequestsHandlers()
            .Select(requestHandlerType => new RequestHandlerMeta(requestHandlerType.FindMediaTRRequestType()!,
                requestHandlerType, requestHandlerType.FindCachedRequestType()));

    private static IEnumerable<RequestHandlerMeta> FilterHandlersForCachedRequest(
        this IEnumerable<RequestHandlerMeta> types) =>
        types.Where(t => typeof(ICachedQuery).IsAssignableFrom(t.RequestType));

    public static IEnumerable<Type> FindAllMediatRRequestsHandlers(this Assembly assembly)
    {
        var result = new List<Type>();
        foreach (var type in assembly.DefinedTypes.Where(t => !t.IsOpenGeneric()))
        {
            var interfaceTypes = type.FindInterfacesThatClose(typeof(IRequestHandler<,>)).ToArray();
            if (interfaceTypes.Length == 0)
            {
                continue;
            }

            if (type.IsConcrete())
            {
                result.Add(type);
            }
        }

        return result;
    }
    
    private static bool IsOpenGeneric(this Type type) => type.IsGenericTypeDefinition || type.ContainsGenericParameters;
    
    private static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType) => 
        FindInterfacesThatClosesCore(pluggedType, templateType).Distinct();

    private static IEnumerable<Type> FindInterfacesThatClosesCore(Type pluggedType, Type templateType)
    {
        if (!pluggedType.IsConcrete()) yield break;

        if (templateType.IsInterface)
        {
            foreach (
                var interfaceType in
                pluggedType.GetInterfaces()
                    .Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
            {
                yield return interfaceType;
            }
        }
        else if (pluggedType.BaseType!.IsGenericType &&
                 (pluggedType.BaseType!.GetGenericTypeDefinition() == templateType))
        {
            yield return pluggedType.BaseType!;
        }

        if (pluggedType.BaseType == typeof(object)) yield break;

        foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.BaseType!, templateType))
        {
            yield return interfaceType;
        }
    }

    private static Type? FindCachedRequestType(this Type requestHandlerType) =>
        requestHandlerType.FindMediaTRRequestType()?
            .FindFirstCachedRequestType();

    private static Type? FindFirstCachedRequestType(this Type requestType)
    {
        if (!typeof(ICachedQuery).IsAssignableFrom(requestType))
        {
            return null;
        }

        var foundType = requestType;
        while (foundType.BaseType != null)
        {
            if (typeof(ICachedQuery).IsAssignableFrom(foundType.BaseType))
            {
                foundType = foundType.BaseType;
            }
            else
            {
                return foundType;
            }
        }

        return foundType;
    }


    private static Type? FindMediaTRRequestType(this Type requestHandlerType)
    {
        var requestHandlerInterfaceType = requestHandlerType
            .GetInterfaces()
            .FirstOrDefault(interfaceType => interfaceType.IsGenericType &&
                                             interfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

        Type? requestType = null;
        if (requestHandlerInterfaceType != null)
        {
            requestType = requestHandlerInterfaceType.GenericTypeArguments[0];
        }

        return requestType;
    }
}