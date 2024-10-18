namespace CraftersCloud.ReferenceArchitecture.Data.Migrations;

public static class DevelopmentConnectionsStrings
{
    private const string DatabaseName = "crafters-cloud-reference-architecture";

    public static string MainConnectionString =>
        $"Server=.;Database={DatabaseName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
}