using Ardalis.SmartEnum;
using SmartEnumExtensions = CraftersCloud.ReferenceArchitecture.Core.SmartEnums.SmartEnumExtensions;

namespace CraftersCloud.ReferenceArchitecture.Domain.Products;

public class ProductStatusId(string name, byte value) : SmartEnum<ProductStatusId>(name, value)
{
    public static readonly ProductStatusId Active = new(nameof(Active), 1);
    public static readonly ProductStatusId Inactive = new(nameof(Inactive), 2);

    [UsedImplicitly(Reason = "Required for Minimal Api custom binding to work")]
    public static bool TryParse(string value, out ProductStatusId result) =>
        SmartEnumExtensions.TryParseByteValueAsString(value, out result);
}