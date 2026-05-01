using Microsoft.Win32;

namespace SiliconLife.Speedy.Manager.Services;

/// <summary>
/// 封装 WPF 系统文件对话框的服务实现。
/// </summary>
public sealed class FileDialogService : IFileDialogService
{
    // ContentType → 默认扩展名映射（对应需求 8.2）
    private static readonly Dictionary<string, string> ContentTypeExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ["json"] = ".json",
        ["text"] = ".txt",
        ["raw"] = ".bin"
    };

    /// <summary>
    /// 根据 ContentType 获取对应的默认文件扩展名。
    /// </summary>
    public static string GetExtensionForContentType(string contentType)
    {
        return ContentTypeExtensions.TryGetValue(contentType, out var ext) ? ext : ".bin";
    }

    /// <inheritdoc/>
    public string? OpenSpkFile()
    {
        var dialog = new OpenFileDialog
        {
            Title = "打开 SpeedyPack 文件",
            Filter = "SpeedyPack 文件 (*.spk)|*.spk|所有文件 (*.*)|*.*",
            FilterIndex = 1,
            CheckFileExists = true,
            CheckPathExists = true
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    /// <inheritdoc/>
    public string? SaveFile(string defaultFileName, string defaultExtension)
    {
        // 确保扩展名以 "." 开头
        var ext = defaultExtension.StartsWith('.') ? defaultExtension : "." + defaultExtension;

        var filter = ext.ToLowerInvariant() switch
        {
            ".json" => "JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*",
            ".txt" => "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
            ".bin" => "二进制文件 (*.bin)|*.bin|所有文件 (*.*)|*.*",
            _ => $"文件 (*{ext})|*{ext}|所有文件 (*.*)|*.*"
        };

        var dialog = new SaveFileDialog
        {
            Title = "导出条目",
            FileName = defaultFileName,
            DefaultExt = ext,
            Filter = filter,
            FilterIndex = 1,
            OverwritePrompt = true
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    /// <inheritdoc/>
    public string? SelectDirectory()
    {
        // WPF 没有内置的 FolderBrowserDialog，使用 OpenFolderDialog（.NET 8+）
        var dialog = new OpenFolderDialog
        {
            Title = "选择目标目录",
            Multiselect = false
        };

        return dialog.ShowDialog() == true ? dialog.FolderName : null;
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> OpenMultipleFiles()
    {
        var dialog = new OpenFileDialog
        {
            Title = "选择要导入的文件",
            Filter = "所有文件 (*.*)|*.*",
            Multiselect = true,
            CheckFileExists = true,
            CheckPathExists = true
        };

        if (dialog.ShowDialog() != true)
            return Array.Empty<string>();

        return dialog.FileNames;
    }
}
