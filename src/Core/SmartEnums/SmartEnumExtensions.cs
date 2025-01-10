using Ardalis.SmartEnum;

namespace CraftersCloud.ReferenceArchitecture.Core.SmartEnums;

public static class SmartEnumExtensions
{
    public static bool TryParseByteValueAsString<T>(string value, out T result) where T : SmartEnum<T>
    {
        if (byte.TryParse(value, out var intValue))
        {
            return SmartEnum<T>.TryFromValue(intValue, out result);
        }

        result = SmartEnum<T>.List.First();
        return false;
    }
}