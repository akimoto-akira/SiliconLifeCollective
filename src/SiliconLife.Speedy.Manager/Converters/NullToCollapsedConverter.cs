using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SiliconLife.Speedy.Manager.Converters;

/// <summary>
/// Converts a null value to Collapsed and a non-null value to Visible.
/// Useful for hiding validation error messages when there is no error.
/// </summary>
public class NullToCollapsedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException($"{nameof(NullToCollapsedConverter)} does not support ConvertBack.");
    }
}
