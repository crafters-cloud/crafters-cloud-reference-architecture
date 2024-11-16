using System.Reflection;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure;

public static class AssemblyFinder
{
    private const string ProjectPrefix = "CraftersCloud.ReferenceArchitecture";
    public static Assembly ApplicationAssembly => FindAssembly("Application");
    public static Assembly ApiAssembly => FindAssembly("Api");
    public static Assembly DomainAssembly => FindAssembly("Domain");
    public static Assembly InfrastructureAssembly => FindAssembly("Infrastructure");

    private static Assembly FindAssembly(string projectSuffix) => Find($"{ProjectPrefix}.{projectSuffix}");

    public static Assembly Find(string assemblyName) => Assembly.Load(assemblyName);
}