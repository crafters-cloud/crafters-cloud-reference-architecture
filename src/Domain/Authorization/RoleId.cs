using CraftersCloud.ReferenceArchitecture.Core;

namespace CraftersCloud.ReferenceArchitecture.Domain.Authorization;

public readonly record struct RoleId(Guid Value) : IStronglyTypedId<RoleId>
{
    public static implicit operator Guid(RoleId roleId) => roleId.Value;

    [UsedImplicitly]
    public static bool TryParse(string value, out RoleId result) =>
        IStronglyTypedId<RoleId>.TryParse(value, out result);

    public override string ToString() => Value.ToString();
}