using System.Reflection;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Txf.VirtualEvents.Common.Tests.MediatR;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.MediatR;

public static class AssemblyTestHelper
{
    public static void Given_RequestIsCached_RequestHandler_MustNotDependOnTransientType(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        var requestAndRequestHandlers =
            assembly.FindAllRequestsAndRequestHandlersThatAreCached();
        foreach (var meta in requestAndRequestHandlers)
        {
            var dependingOn =
                meta.RequestHandlerType.FilterDependingOnTypes(
                    typeof(ICurrentUserProvider),
                    typeof(IHttpContextAccessor));

            dependingOn
                .ShouldBeEmpty($"{meta.RequestHandlerType} must not depend on transient dependencies since the data is cached");
        }
    }

    public static void GivenRequestIsCached_CacheEvictorMustNotBeInThisAssembly_SinceCacheInvalidationWontPickItUp(
        string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        var requestAndRequestHandlers =
            assembly.FindAllRequestsAndRequestHandlersThatAreCached();
        foreach (var meta in requestAndRequestHandlers)
        {
            var cacheEvictors = meta.RequestType.FindCacheEvictors(false);

            cacheEvictors.Count().ShouldBe(0,
                $"{meta.RequestType} should have not have cache evictor defined in same assembly as the cache request");
        }
    }

    public static void GivenRequestIsCached_CacheEvictorMustBeDefined(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        var requestAndRequestHandlers =
            assembly.FindAllRequestsAndRequestHandlersThatAreCached();
        foreach (var meta in requestAndRequestHandlers)
        {
            var cacheEvictors = meta.RequestType.FindCacheEvictors(true);

            Console.WriteLine($"{meta.RequestType} has {cacheEvictors.Count()} cache evictor(s)");

            //TODO: after all cache evictors have been defined this can be uncommented
            //cacheEvictors.Should().HaveCount(1, $"{requestType} should have only one cache evictor defined");
        }
    }

    public static void TestAssemblyContainRequestHandlers(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        assembly.ShouldNotBeNull();
        var requestHandlers = assembly.FindAllMediatRRequestsHandlers();
        requestHandlers.Count().ShouldBeGreaterThan(0, $"assembly {assemblyName} should have MediatR RequestHandlers");
    }

    public static void TestAssemblyDoesNotContainRequestHandlers(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        assembly.ShouldNotBeNull();
        var requestHandlers = assembly.FindAllMediatRRequestsHandlers();
        requestHandlers.Count().ShouldBe(0, $"assembly {assemblyName} should not have MediatR RequestHandlers");
    }
}