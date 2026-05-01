using CommunityToolkit.Mvvm.ComponentModel;
using SiliconLife.Speedy.Manager.Converters;
using SiliconLife.Speedy.Manager.Models;

namespace SiliconLife.Speedy.Manager.ViewModels;

/// <summary>
/// 元数据面板 ViewModel，显示选中条目或目录的详细信息。
/// 对应需求 3.3, 2.6。
/// </summary>
public partial class MetadataPanelViewModel : ObservableObject
{
    /// <summary>选中条目的虚拟路径。</summary>
    [ObservableProperty]
    private string _virtualPath = string.Empty;

    /// <summary>选中条目的内容类型（"json"、"raw"、"text"）。</summary>
    [ObservableProperty]
    private string _contentType = string.Empty;

    /// <summary>选中条目的字节大小格式化字符串（如 "1,234 B"）。</summary>
    [ObservableProperty]
    private string _length = string.Empty;

    /// <summary>选中条目的创建时间。</summary>
    [ObservableProperty]
    private DateTime _createdAt;

    /// <summary>选中条目的最后修改时间。</summary>
    [ObservableProperty]
    private DateTime _updatedAt;

    /// <summary>是否有选中项（控制面板内容的可见性）。</summary>
    [ObservableProperty]
    private bool _hasSelection;

    /// <summary>是否选中的是目录节点（控制目录统计信息的可见性）。</summary>
    [ObservableProperty]
    private bool _isDirectory;

    /// <summary>选中目录下的直接子条目数量（仅目录节点有效）。</summary>
    [ObservableProperty]
    private int _entryCount;

    /// <summary>选中目录下的直接子目录数量（仅目录节点有效）。</summary>
    [ObservableProperty]
    private int _dirCount;

    /// <summary>
    /// 加载条目元数据并更新面板显示。
    /// </summary>
    /// <param name="metadata">条目元数据。</param>
    public void LoadMetadata(EntryMetadata metadata)
    {
        VirtualPath = metadata.VirtualPath;
        ContentType = metadata.ContentType;
        Length = ByteSizeToStringConverter.FormatBytes(metadata.Length);
        CreatedAt = metadata.CreatedAt;
        UpdatedAt = metadata.UpdatedAt;
        IsDirectory = false;
        EntryCount = 0;
        DirCount = 0;
        HasSelection = true;
    }

    /// <summary>
    /// 加载目录信息并更新面板显示（显示子条目数和子目录数）。
    /// </summary>
    /// <param name="path">目录虚拟路径。</param>
    /// <param name="entryCount">直接子条目数量。</param>
    /// <param name="dirCount">直接子目录数量。</param>
    public void LoadDirectoryInfo(string path, int entryCount, int dirCount)
    {
        VirtualPath = path;
        ContentType = string.Empty;
        Length = string.Empty;
        CreatedAt = default;
        UpdatedAt = default;
        IsDirectory = true;
        EntryCount = entryCount;
        DirCount = dirCount;
        HasSelection = true;
    }

    /// <summary>
    /// 清空面板内容（关闭文件或取消选中时调用）。
    /// </summary>
    public void Clear()
    {
        VirtualPath = string.Empty;
        ContentType = string.Empty;
        Length = string.Empty;
        CreatedAt = default;
        UpdatedAt = default;
        IsDirectory = false;
        EntryCount = 0;
        DirCount = 0;
        HasSelection = false;
    }
}
