using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SiliconLife.Speedy.Manager.Converters;

/// <summary>
/// Converts a bool to a <see cref="Visibility"/> value.
/// Default mapping: true → Visible, false → Collapsed.
/// Pass "Invert" (case-insensitive) as the ConverterParameter to reverse the mapping:
///   true → Collapsed, false → Visible.
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool flag = value is bool b && b;

        bool invert = IsInvert(parameter);

        if (invert)
            flag = !flag;

        return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Visibility visibility)
            return false;

        bool result = visibility == Visibility.Visible;

        bool invert = IsInvert(parameter);
        if (invert)
            result = !result;

        return result;
    }

    private static bool IsInvert(object? parameter)
    {
        return parameter is string s &&
               s.Equals("Invert", StringComparison.OrdinalIgnoreCase);
    }
}
