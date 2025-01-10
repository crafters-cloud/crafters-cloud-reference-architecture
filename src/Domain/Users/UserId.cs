using CraftersCloud.ReferenceArchitecture.Core;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

public readonly record struct UserId(Guid Value) : IStronglyTypedId<UserId>
{
    public static implicit operator Guid(UserId id) => id.Value;

    [UsedImplicitly]
    public static bool TryParse(string value, out UserId result) =>
        IStronglyTypedId<UserId>.TryParse(value, out result);

    public override string ToString() => Value.ToString();
}