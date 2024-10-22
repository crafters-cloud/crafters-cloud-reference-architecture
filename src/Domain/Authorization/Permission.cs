using CraftersCloud.Core.Entities;

#pragma warning disable CA1711

namespace CraftersCloud.ReferenceArchitecture.Domain.Authorization;

public class Permission : EntityWithTypedId<PermissionId>
{
    public const int NameMaxLength = 100;
    private readonly IList<Role> _roles = [];
    public string Name { get; private init; } = string.Empty;
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    [UsedImplicitly]
    private Permission() { }

    public Permission(PermissionId permissionId)
    {
        Id = permissionId;
        Name = permissionId.ToString();
    }
}