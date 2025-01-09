using CraftersCloud.Core.Entities;

namespace CraftersCloud.ReferenceArchitecture.Domain.Authorization;

public class Role : EntityWithGuidId
{
    public const int NameMaxLength = 100;
    public static readonly Guid SystemAdminRoleId = new("028e686d-51de-4dd9-91e9-dfb5ddde97d0");

    private IList<Permission> _permissions = [];
    public string Name { get; init; } = string.Empty;
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    public void SetPermissions(IEnumerable<Permission> permissions) => _permissions = permissions.ToList();
}