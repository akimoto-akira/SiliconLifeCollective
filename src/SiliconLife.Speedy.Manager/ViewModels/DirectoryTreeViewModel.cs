using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SiliconLife.Speedy.Manager.Helpers;
using SiliconLife.Speedy.Manager.Models;
using SiliconLife.Speedy.Manager.Services;

namespace SiliconLife.Speedy.Manager.ViewModels;

/// <summary>
/// 目录树 ViewModel，管理 Pack 文件的虚拟目录结构展示与操作。
/// 对应需求 2.1, 2.3, 2.5, 4.4, 6.3, 11.1, 11.2, 11.3, 11.4。
/// </summary>
public partial class DirectoryTreeViewModel : ObservableObject
{
    private readonly IPackService _packService;
    private readonly IFileDialogService _fileDialogService;
    private readonly INotificationService _notificationService;

    // 外部回调，当选中节点变化时通知 MainViewModel 加载内容和元数据
    public event Action<PackEntryNode?>? SelectedNodeChanged;

    public DirectoryTreeViewModel(
        IPackService packService,
        IFileDialogService fileDialogService,
        INotificationService notificationService)
    {
        _packService = packService;
        _fileDialogService = fileDialogService;
        _notificationService = notificationService;
    }

    /// <summary>根节点集合（对应 Pack 文件根路径 "" 下的直接子节点）。</summary>
    public ObservableCollection<PackEntryNode> RootNodes { get; } = new();

    /// <summary>当前选中的节点。</summary>
    [ObservableProperty]
    private PackEntryNode? _selectedNode;

    /// <summary>搜索框文本。</summary>
    [ObservableProperty]
    private string _searchText = string.Empty;

    /// <summary>搜索结果节点列表（所有路径包含搜索文本的节点）。</summary>
    public ObservableCollection<PackEntryNode> SearchResults { get; } = new();

    /// <summary>搜索无结果时是否显示提示。</summary>
    [ObservableProperty]
    private bool _showNoResultsHint;

    /// <summary>
    /// 搜索文本变化时触发：大小写不敏感过滤，更新所有节点的 IsHighlighted，
    /// 自动展开匹配节点的父节点；搜索文本为空时清除所有高亮。
    /// </summary>
    partial void OnSearchTextChanged(string value)
    {
        SearchResults.Clear();

        if (string.IsNullOrEmpty(value))
        {
            // 清除所有高亮
            ClearHighlights(RootNodes);
            ShowNoResultsHint = false;
            return;
        }

        // 大小写不敏感搜索
        var lowerSearch = value.ToLowerInvariant();
        var matchedNodes = new List<PackEntryNode>();
        SearchNodes(RootNodes, lowerSearch, matchedNodes, parentPath: null);

        foreach (var node in matchedNodes)
            SearchResults.Add(node);

        ShowNoResultsHint = matchedNodes.Count == 0;
    }

    private void ClearHighlights(IEnumerable<PackEntryNode> nodes)
    {
        foreach (var node in nodes)
        {
            node.IsHighlighted = false;
            ClearHighlights(node.Children);
        }
    }

    /// <summary>
    /// 递归搜索节点，设置 IsHighlighted，并自动展开匹配节点的父节点。
    /// </summary>
    /// <returns>当前节点或其子节点中是否有匹配项。</returns>
    private bool SearchNodes(
        IEnumerable<PackEntryNode> nodes,
        string lowerSearch,
        List<PackEntryNode> matchedNodes,
        string? parentPath)
    {
        bool anyMatch = false;

        foreach (var node in nodes)
        {
            bool nodeMatches = node.FullPath.ToLowerInvariant().Contains(lowerSearch);
            bool childrenMatch = SearchNodes(node.Children, lowerSearch, matchedNodes, node.FullPath);

            node.IsHighlighted = nodeMatches;

            if (nodeMatches)
                matchedNodes.Add(node);

            // 如果子节点有匹配，展开当前节点
            if (childrenMatch)
                node.IsExpanded = true;

            anyMatch = anyMatch || nodeMatches || childrenMatch;
        }

        return anyMatch;
    }

    /// <summary>
    /// 展开节点并懒加载子节点。
    /// </summary>
    [RelayCommand]
    private void ExpandNode(PackEntryNode node)
    {
        if (!node.IsDirectory || node.IsLoaded)
            return;

        LoadNodeChildren(node);
    }

    /// <summary>
    /// 选中节点并触发 SelectedNodeChanged 事件。
    /// </summary>
    [RelayCommand]
    private void SelectNode(PackEntryNode node)
    {
        SelectedNode = node;
        SelectedNodeChanged?.Invoke(node);
    }

    /// <summary>
    /// 新建条目命令（由 MainViewModel 通过对话框处理）。
    /// </summary>
    [RelayCommand]
    private async Task NewEntryAsync()
    {
        // 实际对话框逻辑由 MainViewModel 协调，此处触发事件
        await Task.CompletedTask;
        NewEntryRequested?.Invoke(SelectedNode?.FullPath ?? string.Empty);
    }

    /// <summary>新建条目请求事件，参数为当前选中目录路径。</summary>
    public event Action<string>? NewEntryRequested;

    /// <summary>
    /// 删除条目命令。
    /// </summary>
    [RelayCommand]
    private async Task DeleteEntryAsync(PackEntryNode node)
    {
        if (node.IsDirectory)
        {
            _notificationService.ShowError("无法删除", "不能直接删除目录，请先删除目录下的所有条目。");
            return;
        }

        bool confirmed = await _notificationService.ShowConfirmAsync(
            "确认删除",
            $"确定要删除条目 \"{node.FullPath}\" 吗？此操作不可撤销。");

        if (!confirmed) return;

        try
        {
            await Task.Run(() => _packService.DeleteEntry(node.FullPath));
            RefreshTree();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError("删除失败", $"无法删除条目：{ex.Message}");
        }
    }

    /// <summary>
    /// 导入文件命令（由 MainViewModel 通过对话框处理）。
    /// </summary>
    [RelayCommand]
    private async Task ImportFilesAsync()
    {
        await Task.CompletedTask;
        ImportFilesRequested?.Invoke(SelectedNode?.FullPath ?? string.Empty);
    }

    /// <summary>导入文件请求事件，参数为当前选中目录路径（作为默认前缀）。</summary>
    public event Action<string>? ImportFilesRequested;

    /// <summary>
    /// 导出条目命令。
    /// </summary>
    [RelayCommand]
    private async Task ExportEntryAsync(PackEntryNode node)
    {
        if (node.IsDirectory)
        {
            await ExportDirectoryAsync(node);
            return;
        }

        // 根据 ContentType 确定默认扩展名
        var metadata = _packService.GetMetadata(node.FullPath);
        var ext = GetDefaultExtension(metadata?.ContentType ?? "raw");
        var defaultName = node.Name;

        var targetPath = _fileDialogService.SaveFile(defaultName, ext);
        if (targetPath == null) return;

        try
        {
            await _packService.ExportEntryAsync(node.FullPath, targetPath);
            _notificationService.ShowInfo("导出成功", $"条目已导出到：{targetPath}");
        }
        catch (Exception ex)
        {
            _notificationService.ShowError("导出失败", $"无法导出条目：{ex.Message}");
        }
    }

    /// <summary>
    /// 导出目录命令。
    /// </summary>
    [RelayCommand]
    private async Task ExportDirectoryAsync(PackEntryNode node)
    {
        var targetDir = _fileDialogService.SelectDirectory();
        if (targetDir == null) return;

        try
        {
            await _packService.ExportDirectoryAsync(node.FullPath, targetDir);
            _notificationService.ShowInfo("导出成功", $"目录已导出到：{targetDir}");
        }
        catch (Exception ex)
        {
            _notificationService.ShowError("导出失败", $"无法导出目录：{ex.Message}");
        }
    }

    /// <summary>
    /// 重新从 IPackService 加载整棵树。
    /// </summary>
    public void RefreshTree()
    {
        RootNodes.Clear();

        if (!_packService.IsOpen) return;

        LoadChildrenInto(RootNodes, string.Empty);
    }

    /// <summary>
    /// 重新加载指定节点的子节点。
    /// </summary>
    public void RefreshNode(PackEntryNode node)
    {
        node.Children.Clear();
        node.IsLoaded = false;
        LoadNodeChildren(node);
    }

    /// <summary>
    /// 清空树（关闭文件时调用）。
    /// </summary>
    public void Clear()
    {
        RootNodes.Clear();
        SelectedNode = null;
        SearchText = string.Empty;
        SearchResults.Clear();
        ShowNoResultsHint = false;
    }

    private void LoadNodeChildren(PackEntryNode node)
    {
        LoadChildrenInto(node.Children, node.FullPath);
        node.IsLoaded = true;
        node.IsExpanded = true;
    }

    private void LoadChildrenInto(ObservableCollection<PackEntryNode> collection, string directoryPath)
    {
        // 加载子目录
        var dirs = _packService.ListDirectories(directoryPath);
        foreach (var dir in dirs)
        {
            var dirNode = new PackEntryNode(dir, isDirectory: true);
            collection.Add(dirNode);
        }

        // 加载子条目
        var entries = _packService.ListEntries(directoryPath);
        foreach (var entry in entries)
        {
            var entryNode = new PackEntryNode(entry, isDirectory: false);
            entryNode.IsLoaded = true; // 叶节点无需懒加载
            collection.Add(entryNode);
        }
    }

    private static string GetDefaultExtension(string contentType) => contentType switch
    {
        "json" => ".json",
        "text" => ".txt",
        _ => ".bin"
    };
}
