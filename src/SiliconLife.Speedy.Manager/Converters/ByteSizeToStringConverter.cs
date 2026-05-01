using System.Globalization;
using System.Windows.Data;

namespace SiliconLife.Speedy.Manager.Converters;

/// <summary>
/// Converts a numeric byte count (long/int/ulong) to a human-readable size string.
/// Format rules:
///   &lt; 1 KB  → "1,234 B"   (comma-formatted integer, no decimal)
///   &lt; 1 MB  → "1.2 KB"    (one decimal place)
///   &lt; 1 GB  → "2.3 MB"    (one decimal place)
///   ≥ 1 GB  → "1.5 GB"    (one decimal place)
/// </summary>
public class ByteSizeToStringConverter : IValueConverter
{
    private const long KB = 1024L;
    private const long MB = 1024L * KB;
    private const long GB = 1024L * MB;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        long bytes;

        try
        {
            bytes = System.Convert.ToInt64(value);
        }
        catch
        {
            return value?.ToString() ?? string.Empty;
        }

        return FormatBytes(bytes);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException($"{nameof(ByteSizeToStringConverter)} does not support ConvertBack.");
    }

    /// <summary>
    /// Formats a byte count into a human-readable string.
    /// </summary>
    public static string FormatBytes(long bytes)
    {
        if (bytes < KB)
            return $"{bytes:N0} B";

        if (bytes < MB)
            return $"{bytes / (double)KB:F1} KB";

        if (bytes < GB)
            return $"{bytes / (double)MB:F1} MB";

        return $"{bytes / (double)GB:F1} GB";
    }
}
