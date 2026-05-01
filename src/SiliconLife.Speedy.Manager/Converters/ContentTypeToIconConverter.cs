using System.Globalization;
using System.Windows.Data;

namespace SiliconLife.Speedy.Manager.Converters;

/// <summary>
/// Converts a ContentType string (or IsDirectory bool) to an icon resource key string.
/// When used with a PackEntryNode, pass IsDirectory as the value and ContentType as the parameter,
/// or pass the ContentType string directly.
/// </summary>
public class ContentTypeToIconConverter : IValueConverter
{
    /// <summary>
    /// Converts a value to an icon resource key string.
    /// </summary>
    /// <param name="value">
    ///   If bool: treated as IsDirectory flag.
    ///   If string: treated as ContentType ("json", "text", "raw").
    /// </param>
    /// <param name="parameter">
    ///   Optional ContentType string when value is a bool (IsDirectory).
    /// </param>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // If value is a bool, it represents IsDirectory
        if (value is bool isDirectory)
        {
            if (isDirectory)
                return "FolderIcon";

            // Not a directory — use ContentType from parameter if provided
            var contentType = parameter as string;
            return GetIconForContentType(contentType);
        }

        // If value is a string, treat it as ContentType
        if (value is string contentTypeStr)
        {
            return GetIconForContentType(contentTypeStr);
        }

        return "FileIcon";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException($"{nameof(ContentTypeToIconConverter)} does not support ConvertBack.");
    }

    private static string GetIconForContentType(string? contentType)
    {
        return contentType?.ToLowerInvariant() switch
        {
            "json" => "JsonIcon",
            "text" => "TextIcon",
            "raw"  => "RawIcon",
            _      => "FileIcon"
        };
    }
}
