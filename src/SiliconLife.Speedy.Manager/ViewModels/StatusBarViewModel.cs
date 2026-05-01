using CommunityToolkit.Mvvm.ComponentModel;
using SiliconLife.Speedy.Manager.Converters;
using SiliconLife.Speedy.Manager.Models;

namespace SiliconLife.Speedy.Manager.ViewModels;

/// <summary>
/// 状态栏 ViewModel，显示当前打开文件的基本信息与操作状态消息。
/// 对应需求 10.1, 12.3。
/// </summary>
public partial class StatusBarViewModel : ObservableObject
{
    /// <summary>当前打开的文件路径，未打开时为空字符串。</summary>
    [ObservableProperty]
    private string _filePath = string.Empty;

    /// <summary>文件大小的格式化字符串（如 "2.3 MB"），未打开时为空字符串。</summary>
    [ObservableProperty]
    private string _fileSize = string.Empty;

    /// <summary>Pack 文件中的总条目数量。</summary>
    [ObservableProperty]
    private int _totalEntries;

    /// <summary>当前文件是否以只读模式打开。</summary>
    [ObservableProperty]
    private bool _isReadOnly;

    /// <summary>状态栏消息文本（如"正在刷新..."、"刷新完成"等）。</summary>
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>
    /// 根据 PackFileInfo 更新状态栏显示信息。
    /// </summary>
    /// <param name="info">Pack 文件信息。</param>
    public void UpdateFromFileInfo(PackFileInfo info)
    {
        FilePath = info.FilePath;
        FileSize = ByteSizeToStringConverter.FormatBytes(info.FileSize);
        TotalEntries = info.TotalEntries;
    }

    /// <summary>
    /// 设置状态栏消息文本。
    /// </summary>
    /// <param name="message">要显示的消息。</param>
    public void SetStatus(string message)
    {
        StatusMessage = message;
    }

    /// <summary>
    /// 清空状态栏所有显示内容（关闭文件后调用）。
    /// </summary>
    public void Clear()
    {
        FilePath = string.Empty;
        FileSize = string.Empty;
        TotalEntries = 0;
        IsReadOnly = false;
        StatusMessage = string.Empty;
    }
}
