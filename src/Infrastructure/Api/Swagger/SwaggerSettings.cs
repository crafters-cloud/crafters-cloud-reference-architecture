using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Api.Swagger;

[UsedImplicitly]
public class SwaggerSettings
{
    public const string SectionName = "Swagger";
    public bool Enabled { get; set; }
}