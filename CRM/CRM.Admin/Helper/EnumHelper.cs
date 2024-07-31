using System.ComponentModel;

namespace CRM.Admin.Helper;

public static class EnumHelper
{
    public static string GetEnumDescriptionWithName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute;

        var description = attribute?.Description ?? value.ToString();
        return $"{value} - {description}";
    }
}