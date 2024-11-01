namespace CraftersCloud.ReferenceArchitecture.Infrastructure.KeyVault;

[UsedImplicitly]
public class KeyVaultSettings
{
    public const string SectionName = "KeyVault";
    public bool Enabled { get; set; }
    public string Name { get; set; } = string.Empty;
}