using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SiliconLife.Speedy.Manager.Converters;
using SiliconLife.Speedy.Manager.Models;

namespace SiliconLife.Speedy.Manager.ViewModels.Dialogs;

/// <summary>
/// 文件信息对话框 ViewModel，只读展示 PackFileInfo 的所有字段。
/// 对应需求 10.2。
/// </summary>
public partial class FileInfoDialogViewModel : ObservableObject
{
    /// <summary>文件路径。</summary>
    [ObservableProperty]
    private string _filePath = string.Empty;

    /// <summary>文件大小格式化字符串（如 "2.3 MB"）。</summary>
    [ObservableProperty]
    private string _fileSize = string.Empty;

    /// <summary>Magic 标识字符串。</summary>
    [ObservableProperty]
    private string _magic = string.Empty;

    /// <summary>文件版本号。</summary>
    [ObservableProperty]
    private ushort _version;

    /// <summary>文件标志位。</summary>
    [ObservableProperty]
    private ushort _flags;

    /// <summary>目录区偏移量。</summary>
    [ObservableProperty]
    private long _directoryOffset;

    /// <summary>目录区长度（字节）。</summary>
    [ObservableProperty]
    private int _directoryLength;

    /// <summary>总条目数量。</summary>
    [ObservableProperty]
    private int _totalEntries;

    /// <summary>JSON 类型条目数量。</summary>
    [ObservableProperty]
    private int _jsonEntries;

    /// <summary>Raw 类型条目数量。</summary>
    [ObservableProperty]
    private int _rawEntries;

    /// <summary>Text 类型条目数量。</summary>
    [ObservableProperty]
    private int _textEntries;

    /// <summary>
    /// 从 PackFileInfo 加载所有字段。
    /// </summary>
    /// <param name="info">Pack 文件信息。</param>
    public void LoadFromFileInfo(PackFileInfo info)
    {
        FilePath = info.FilePath;
        FileSize = ByteSizeToStringConverter.FormatBytes(info.FileSize);
        Magic = info.Magic;
        Version = info.Version;
        Flags = info.Flags;
        DirectoryOffset = info.DirectoryOffset;
        DirectoryLength = info.DirectoryLength;
        TotalEntries = info.TotalEntries;
        JsonEntries = info.JsonEntries;
        RawEntries = info.RawEntries;
        TextEntries = info.TextEntries;
    }

    /// <summary>
    /// 关闭对话框命令。
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        DialogCloseRequested?.Invoke();
    }

    /// <summary>请求关闭对话框的事件。</summary>
    public event Action? DialogCloseRequested;
}
