using System.Reflection;
using CraftersCloud.Core.AspNetCore.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Init;

public static class FluentValidationStartupExtensions
{
    public static void AppConfigureFluentValidation(this IApplicationBuilder _) =>
        ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

    public static IServiceCollection AppAddFluentValidation(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblies(
        [
            AssemblyFinder.DomainAssembly, AssemblyFinder.ApplicationServicesAssembly, AssemblyFinder.ApiAssembly
        ]);
}