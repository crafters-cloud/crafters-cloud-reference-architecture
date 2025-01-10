using CraftersCloud.ReferenceArchitecture.Core;

namespace CraftersCloud.ReferenceArchitecture.Common.Tests.StronglyTypedIds;

internal class StronglyTypedIdWriteOnlyJsonConverter<TId> : WriteOnlyJsonConverter<TId>
    where TId : struct, IStronglyTypedId<TId>
{
    public override void Write(VerifyJsonWriter writer, TId value) => writer.WriteValue(value.Value);
}