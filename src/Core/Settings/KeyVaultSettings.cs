using JetBrains.Annotations;

namespace CraftersCloud.ReferenceArchitecture.Core.Settings;

[UsedImplicitly]
public class KeyVaultSettings
{
    public const string SectionName = "KeyVault";
    public bool Enabled { get; set; }
    public string Name { get; set; } = string.Empty;
}