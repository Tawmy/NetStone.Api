using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NetStone.Common.Extensions;

public static class EnumExtension
{
    /// <summary>
    ///     A generic extension method that aids in reflecting
    ///     and retrieving any attribute that is applied to an `Enum`.
    /// </summary>
    private static TAttribute? GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        var displayAttr = enumValue.GetAttribute<DisplayAttribute>();
        return displayAttr?.Name ??
               throw new ArgumentNullException(nameof(enumValue), "Enum does not have a display name.");
    }

    public static bool TryGetDisplayName(this Enum enumValue, out string? value)
    {
        var displayAttr = enumValue.GetAttribute<DisplayAttribute>();
        value = displayAttr?.Name;
        return value is not null;
    }

    public static string GetDisplayDescription(this Enum enumValue)
    {
        var displayAttr = enumValue.GetAttribute<DisplayAttribute>();
        return displayAttr?.Description ??
               throw new ArgumentNullException(nameof(enumValue), "Enum does not have a display description.");
    }

    public static bool TryGetDisplayDescription(this Enum enumValue, out string? value)
    {
        var displayAttr = enumValue.GetAttribute<DisplayAttribute>();
        value = displayAttr?.Description;
        return value is not null;
    }
}