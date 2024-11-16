using System.Security.Claims;
using System.Security.Principal;
using Autofac;
using Microsoft.AspNetCore.Http;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Autofac;

public static class ContainerBuilderExtensions
{
    public static void AppRegisterModules(this ContainerBuilder builder)
    {
        builder.RegisterAssemblyModules(AssemblyFinder.InfrastructureAssembly);

        builder.RegisterModule(new ServiceModule
        {
            Assemblies =
            [
                AssemblyFinder.Find("CraftersCloud.Core.Infrastructure"),
                AssemblyFinder.ApplicationAssembly,
                AssemblyFinder.InfrastructureAssembly
            ]
        });
    }

    public static void AppRegisterClaimsPrincipalProvider(this ContainerBuilder builder) =>
        builder.Register(GetPrincipalFromHttpContext)
            .As<IPrincipal>().InstancePerLifetimeScope();

    private static ClaimsPrincipal GetPrincipalFromHttpContext(IComponentContext c)
    {
        var httpContextAccessor = c.Resolve<IHttpContextAccessor>();
        if (httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }

        var user = httpContextAccessor.HttpContext.User;
        return user;
    }
}