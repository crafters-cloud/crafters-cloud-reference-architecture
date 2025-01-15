using CraftersCloud.Core.Entities;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.ReferenceArchitecture.Domain.Authorization;

[StronglyTypedId(ValueKind.Guid)]
public readonly partial record struct RoleId;

public class Role : EntityWithTypedId<RoleId>
{
    public const int NameMaxLength = 100;
    public static readonly RoleId SystemAdminRoleId = RoleId.Create(new Guid("028e686d-51de-4dd9-91e9-dfb5ddde97d0"));

    private Role() { }

    private IList<Permission> _permissions = [];
    public string Name { get; init; } = string.Empty;
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    public void UpdatePermissions(IEnumerable<Permission> permissions) => _permissions = permissions.ToList();

    public static Role Create(string name) => new() { Id = RoleId.CreateNew(), Name = name };
}