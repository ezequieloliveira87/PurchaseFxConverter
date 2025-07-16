namespace PurchaseFxConverter.Domain.Extensions;

public static class EnumExtension
{
    public static string GetEnumDescription(this Enum enumerationValue)
    {
        var type = enumerationValue.GetType();
        var member = type.GetMembers().FirstOrDefault(w => w.Name == Enum.GetName(type, enumerationValue));
        var attribute =
            member?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        return attribute?.Description != null ? attribute.Description : enumerationValue.ToString();
    }
}