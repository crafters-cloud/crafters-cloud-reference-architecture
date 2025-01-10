using CraftersCloud.Core.Entities;

namespace CraftersCloud.ReferenceArchitecture.Core;

public interface IStronglyTypedId<TId> where TId : struct, IStronglyTypedId<TId>
{
    Guid Value { get; protected init; }

    public static TId CreateNew() => Create(SequentialGuidGenerator.Generate());

    public static TId Create(Guid value) => new() { Value = value };

    public static bool TryParse(string value, out TId result)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            result = default;
            return false;
        }

        result = Create(guid);
        return true;
    }
}